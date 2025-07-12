using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pro4Soft.MobileDevice.Plumbing.Infrastructure
{
	public static class PropMapper<TInput, TOutput> where TInput: class where TOutput: class
	{
		private static readonly Func<TInput, TOutput> Cloner;
		private static readonly Action<TInput, TOutput> Copier;

		private static readonly IEnumerable<PropertyInfo> SourceProperties;
		private static readonly IEnumerable<PropertyInfo> DestinationProperties;

		static PropMapper()
		{
			DestinationProperties = typeof(TOutput)
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(prop => !Attribute.IsDefined(prop, typeof(CopyIgnoreAttribute)))
				.Where(prop => prop.CanWrite);
			SourceProperties = typeof(TInput)
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(prop => !Attribute.IsDefined(prop, typeof(CopyIgnoreAttribute)))
				.Where(prop => prop.CanRead);

			Cloner = CreateCloner();
			Copier = CreateCopier();
		}

		private static Func<TInput, TOutput> CreateCloner()
		{
			if (typeof(TOutput).GetConstructor(Type.EmptyTypes) == null) return ((x) => default(TOutput));

			var input = Expression.Parameter(typeof(TInput), "input");

			var memberBindings = SourceProperties.Join(DestinationProperties,
				sourceProperty => sourceProperty.Name,
				destinationProperty => destinationProperty.Name,
				(sourceProperty, destinationProperty) =>
					(MemberBinding)Expression.Bind(destinationProperty,
						Expression.Property(input, sourceProperty)));

			var body = Expression.MemberInit(Expression.New(typeof(TOutput)), memberBindings);
			var lambda = Expression.Lambda<Func<TInput, TOutput>>(body, input);
			return lambda.Compile();
		}

		private static Action<TInput, TOutput> CreateCopier()
		{
			var input = Expression.Parameter(typeof(TInput), "input");
			var output = Expression.Parameter(typeof(TOutput), "output");

			var memberAssignments = SourceProperties.Join(DestinationProperties,
				sourceProperty => sourceProperty.Name,
				destinationProperty => destinationProperty.Name,
				(sourceProperty, destinationProperty) => Expression.Assign(Expression.Property(output, destinationProperty), Expression.Property(input, sourceProperty)));

			var body = Expression.Block(memberAssignments);
			var lambda = Expression.Lambda<Action<TInput, TOutput>>(body, input, output);
			return lambda.Compile();
		}

		public static TOutput From(TInput input)
		{
			return input == null ? default : Cloner(input);
		}

		public static TOutput CopyTo(TInput source, TOutput destination)
		{
			if (source == null)
				return null;
			Copier(source, destination);
			return destination;
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class CopyIgnoreAttribute : Attribute
	{

	}
}