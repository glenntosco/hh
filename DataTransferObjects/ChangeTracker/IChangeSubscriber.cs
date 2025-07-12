using System;
using System.Threading.Tasks;

namespace Pro4Soft.Common.ChangeTracker
{
	public interface IChangeSubscriber
	{
		int Precedence { get; }
		Task HandleChange(object sender, ObjectChangeEvent changeEvent, object entityObject, bool beforeSave);
    }
}