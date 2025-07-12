namespace Pro4Soft.DataTransferObjects
{
    public static class Notes
    {
        public static string ReleaseNotes => @"
2.15.9
- Reworked One scan replenishment screen to give a better visibility of orders roll up and Inventory availability
- Introduced a surrogate Api Key for Integration API Gateway
- Added a new configuration both on a global and client level which controls whether entities should automatically upload on closure
- Added limited support for User defined fields. Currently available for Pickticket header, Purchase order header and Customer return header
- Added support for grid sticky columns
2.15.8
- Added additional screens that allows for easier replenishment of One Scan e-comm orders
- Changed menu structure to consolidate E-Comm processing into a single submenu for clarity
- Failed One Scan e-comm orders will automatically suspend if a shipping label fails to print
- Added a way to create folder structure within Custom dashboards by separating folders with '|' character
- Added a menu search control
2.15.7
- Added Redis Caching for increased performance
- Optimized Grids to retrieve less data which improves performance of the system
- Removed Excel export feature for certain grids which may result in excessive reads from the database
2.15.6
- Added a way to pre-rate one line orders to optimize small parcel shipping
- Added One Scan shipping for small parcel/e-comm orders
- Added a new Configurations section that controls various flavours of One Scan shipping
2.15.5
- Added Driver name to Pick ticket while shipping for private fleet/external
- Allowed assigning carrier on Pick ticket
- Added Tax Id to Carrier
- Extended precision of Rate on Billing Rule to allow small fractional rates (6 decimal points)
- Added a new feature which allows holding specific inventory from allocation to a user
2.15.4
- Added a way to assign LPNs to Totes on Shipping Station screen for various carriers without having to do that on Handheld
- Added a way to use @Rate variable on 3PL Billing Profiles
- Some advanced 3PL Billing profile screens were hidden by default to prevent users from making unintended changes
- Added detailed inventory report which surfaces quantity and detailed information like lots, serials and packsizes
- Surfaced attribute control visibility on product list
- Added a way to designate Client to Custom Action
2.15.3
- Added new Daily inventory snapshot report that records inventory for the passed 60 days
- Added configuration which allows failed upload document to be re-uploaded again for a certain number of attempts
2.15.2
- Added Email notification settings to a customer level. Emailing Packslips, BOLs, RMAs
2.15.1
- Added support for latest Android SDK version
- Updated Handheld API to allow new permissions for Photo capture
2.14.5
- Added Password policy support
- Added automated password expiration and recovery
2.14.4
- Added new settings to Customer/Client/Global which control whether Carton content labels are required
- Added new settings to Customer/Client/Global which control whether to print Packslip upon generating a Carrier label
- Enhanced security by adding Access controls to module details screen
- Added failed login throttling
2.14.3
- Added Pallet printing template and ability to print pallets from Staging/Truckload/Pickticket views
- Added configuration which controls aggregator for pallet numbering
- Added configuration on customer/client/global levels which control whether pallets are required to be printed
- Added expiry inbound and outbound allowances on a product level
- Added UoM column to expiry grid
2.14.2
- Added additional dropship information on Pickticket header and line level
- Reworked Webhook, multiple webhooks are not supporting including Client specific Webhooks
- Added additional check to document level to prevent duplicated document numbering
2.14.1
- Changed allocation precedence in bulk bins from LPN name to LIFO
- Added Gauges to reporting
- Extended Dashboard filter capabilities to support multiple data types (date, string, number)
- Added support for additional columns in grids (Currency, Percentage)
- Added additional Handheld screen - Tote content
2.13.1
- Android 13 support (API level 33)
- Added stage location contents lookup
2.12.3
- Added Four wall report
- Added configurations that control which information is sycnrhonzed and shown on Four wall report
2.12.2
- Added enhanced production options
2.12.1
- Added scannable Tracking number to Customer Returns
- Added a new Carrier Pickup handheld screen
- In addition to SKU and UPC, added a facility to scan BarcodeValue of a Product
- Upgraded to a newer version of OS (31)
2.11.9
- Added additional mapping for Receiving slip
- Improved UI when creating Scheduled Reports
- Opened Comments field for changes on PO closure
- Added Comments on Receiving slip
2.11.8
- Added 'Auto ship small parcel' flag on a user level
2.11.7
- Improved Ecom dashboard to support unallocated single unit parcels
- Added ability to set carton packsize in Ecom dashboard
- Improved Small parcel screen which now allows to scan/change Carton size
2.11.6
- Added Scheduled reports
- Added additional fields and configurations to enrich comercial invoice report
2.11.5
- Added history visibility to Packsizes
- Added a way to handle totes by carrier tracking number
- Added Ecom dashboard, a new way of processing single line, single unit SmallParcel orders in bulk
- Added actionable KPIs to E-comm dashboard and Pickticket list
- Improved Inventory visility and added Task generation controls on Product details screen
2.11.4
- Improved Malvern integration. Added address verification and rating for Residential deliveries
- Improved Tote staging processing while shipping using small parcel
- Added new report type: Commercial Invoice that is available for both Pick tickets and Totes
- Added automatic printing of Commercial invoices for international shipments if carrier cannot do electric submission
- Extended Packsize breakdown to allow breaking to inner packs
2.11.3
- Added a facility to import and export bundle components
- Improved UI for bundle components management
2.11.2
- Introduced configuration which controls whether BOL/MBOL could be shipping without charge terms
- Added precedence facility to shipping rules when a Pick ticket resolves to more than one rule
- Added infrastructure which allows pick ticket small parcel changes to propagate to totes
2.11.1
- Added collection of Dims/Weight on production
- Improved truck load shipping verifications
- Prevent changing shipping method after order has been shipped
2.10.2
- Removed excessive links for non Administrator users
- Changed Apply tags screen to be more user friendly
- Created restock user task automation for min/max bins
2.10.1
- Discontinued Kitting, Production should be used instead
- Reworked 3PL Billing
- Added a way to copy shipping settings to totes from pick ticket
- Added a way to create custom 3PL invoice templates
- Added a way to pick multiple Picktickets using Full Pack Picking at the same time
2.09.2
- Modified UI to aid with long running operations
- Improved performance of shipping of BOLs and Master BOLs
- Optimized and improved visibility of Auto picking
2.09.1
- Introduced a new step in Small Parcel shipping: Carrier pickup and manifest
- Removed redundant link between Truck Loads and Pick tickets
- Removed unused 'Mark for cycle count' option on Handheld skipping
- Added a new way to auto generate truck loads based on grouping algorithm
2.08.6
- Added 'History search' utility which allows searching deleted records
- Moved 'Export to XLSX' and 'Reset Grid' button further apart
- Moved ability to build packsizes duriong production
- Added ability to suspect orders that were shorted during picking
- Added two additional fields to pick ticket - Appt number and Appt date
- Relaxed restriction on Production orders to have duplicate Consume/Produce lines
2.08.5
- Introduced a new configuration setting which controls whether shorted orders are allowed to be shipped
- Added BillTo information to Shipping rules
2.08.4
- Introduced a shipping options default facility based on shipping rules (client/customer/scac/zip)
- Per above, removed all shipping related default that used to be setup on the customer level
- Allowed multiple pick tickets (same customer/address) to be full pack picked to the same pallet
- Added Bin quarantine override for allocation
- QR Code scanning is always enabled for 'sa' user
2.08.3
- Added a way to create custom Packslip/Proforma/Receiving/RMA reports
- Above reports could be applied to Tenant -> Client -> Customer/Vendor
2.08.2
- Added LotNumber field to PO line to force receiving specific lots
- Added a set of configurations which controls allowing custom document numbering
- Upgraded to a newer version of EasyPost api
2.08.1
- Optimized performance of full pack picking
- Improved cartonization screen, added additional relevant columns
- Implemented new mobile app versioning to support Update notification
2.07.1
- Emergency Handheld release to suppress version update check
- Extension to Production
- Surfaced additional fields on Production List to show finished/disassembled product
2.06.1
- Reworked Production module. Old productino is now Kitting
- Added additional handheld screens to support new production
- Added additional configurations that drive production
- Move Workflows to Setup
2.05.1
- General cumulative fixes
2.04.1
- Enabled ability to do packsize breakdown to an LPN instead of a pickable bin
2.03.1
- Additional work to enable Malvern shipping
- Improved performance while handling large, cartonized orders
2.02.1
- Added Commercial Invoice generation for Small Parcel shipping
- Added a way to reprint Commecrial invoice from Web UI
2.01.2
- Major version update
- Substantial changes to backend infrastructure
- Address change to Subdomains
- Added Direct Move Bin suggestion flag - will propose bins on product moves
1.45.1
- Added configuration that controls maximum ordered quantity for full pack picking
- Improved performance of Pick Ticket details screen
1.44.1
- Added quoted weight fields to capture quoted shipping costs
- Added invoiced weight/cube fields to capture actual invoiced costs
- Added Print LPNs option on Staging LPNs screen
- Improved Cartonization performance for order needed a large number of boxes
1.43.1
- Extended Lot allocation capabilities on Pick Ticket line level
- Added Background allocation for Packsize breadkdown/LPN Letdown to improve performance
1.42.1
- Added a way to record an invoice number when capturing shipping costs on Truck load
- Added a configuration Client/Customer/Vendor which controls Back order creation for PO/Pick tickets
1.41.1
- Added additional fields to product master needed to generate customs documentation
- Added automatic Packslip and Truckload printing
1.40.1
- Added a facility to track quoted and actual shipping costs for Truck Loads
- Added a way to add additional shipping options (Residential, SOD, etc...) for small parcel
1.39.1
- Added expanded set of configuration for Small Parcel shipping
- Added 3rd Party shipping/billing options
- Added various payment types when shipping Small Parcel
- Added ability to support international shipping using Int. Tax Id
1.38.1
- Added Malvern small parcel shipping broker
- Moved Malvern/EasyPost small parcel configuration to a client level
1.37.2
- Fixed Directed putaway query to consume different parameters
- Fixed Android GPS collection when screen goes/device goes to sleep
1.37.1
- Added an option to Unpick whole Pick tickets instead of just one Tote
- Added a facility to assign zones and bins to a client
- Added a facility to specify allowed product categories for zones/bins
- Added Receiving Putaway Bin suggestion flag - will propose bins during PO receiving
- Added Returns Putaway Bin suggestion flag - will propose bins during RMA receiving
- Added Advanced section to Configuration which contains SQL queries that drive putaway logic
- Added Production Workflow management
- Added Workflow designer and ability to assign workflows to BOM/Client
1.36.1
- Moved Audit records from Mongo DB to SQL for easier reporting
- Added ShipTo name in Pick ticket/Tote selection while building Truck loads
- Added Staging LPNs report for visibility into truck load planning
- Added a setting which controls whether a BOL document could be printed without signature or not
1.35.1
- Added ability to restrict expiring product on PO and RMA Receiving
- Added additional configuration settings that control expiry allowance
- Added settings to Vendor and Customer that control expiry allowance
1.34.1
- Added Percentage picked to Pick ticket header
- Added a 'Back to Picking' function which allows taking shorted Rating orders back to picking
- Added new Handheld function - LPN merge which allows consolidating staged LPNs
1.33.1
- Added new handheld function to Adjust Out full bin/LPN without scanning product
- Added ability to download most recent APK directly without Google Play Store
- Added Help button with Reseller's information
- Added additional information fields to pickticket detail screen to help with load planning
- Added additional information to packslip report
1.32.1
- Added facility to assign/unassign truck loads to pick tickets even before pick tickets are picked or allocated
- Added a separate settings for handheld weight/dims from default reporting UoM
1.31.1
- Added total number of cartons to Packslip report
- Added total weight (incl. dunnage) and carton weights to Packslip report
- Cosmetic fixes to BOL report
- Added facility to create Master BOL for multiple customers
- Added a restriction for zone names. Now cannot create zones with the same name
- Added an option to do Bulk Updates to Pick tickets and Purchase orders
- Added facility to configure dimensions data to Bins/Zones
- Added new handheld screens to capture dimensions and weight of Totes and LPNs
1.30.1
- Added new handheld tag list screen to fulfillment
1.29.1
- Added facility to activate and deactivate users to temporarily prohibit access to the system
- Added Appointment date to pick tickets
- Added Must Arrive date to pick tickets
- Enabled printing tags from the pick ticket list
- Made comments editable past Draft state
- Surfaced comments in the pick ticket grid
- Added facility to run an import over existing tenants to retain previous settings (ex: integrations or print station)
- Added ability to change product attribute control even for products that have transactions
1.28.1
- Decoupled Email sending to improve performance when emails are enabled
- Improved Allocation performance on large orders
1.27.1
- Added facility to change allocated quantities and partially release reserved stock
- Added facility to Unallocate individual lines
- Carrier now propagates to Pick Ticket and Tote when shipping Truck loads
1.26.1
- Added pick skipping to Production picking
- Minor changes to component consumptions in Production when using By Recipe option
- Added Handheld activity dashboard
- Added facility to redirect handheld to an alternative cloud server on tenant selection
1.25.1
- Added facility to maintain min/max inventory per Bin for product dedicated bins
- Added a way to preview handheld screen of an active user session
1.24.1
- Added ability to setup zone with Single Bin per Product (to support POS)
- Added ability to setup bins with dedicated products (to support POS)
1.23.1
- Extended support for older version of Android (Lollipop sdk v19)
- Added a configuration that controls whether to display Product locations on Product Move
- Added Skip button to Product Letdown handheld option
- Modified LPN Letdown to display all LPNs that are in the queue instead of just the first one
1.22.1
- Added facility to re-allocate orders that have already started being picked
- Added facility to move allocated stock (based on configuration)
- Added Quantity prompt on Packsize Break down handheld function
1.21.1
- Added additional Packsize handling option to Zones/Bins. Zones/Bins could be setup to auto break Packs to Eaches
- Added Letdown by product handheld function to avoid pulling down the whole pallet
1.20.1
- Added additional allocation flavour for Packsize controlled items
- Reworked the way Reseller/Distributor model works and how licenses are assigned
- Introduced new Picking mode - Carton Picking (pick individual cartonized boxes)
- Renamed former 'Carton Pick' to 'Full pack picking'
1.19.1
- Fixed Packsize convert Handheld function
- Fixed Customer return data entry for packsize controlled items
- Added Barcode value to Products and Packsizes with various symbologies (ITF-14, EAN-13, UPC-A, Code 128)
- Added First Pick to pick ticket list for easier waving of high volume single pick orders (e-comm)
- Added Address Override option on pick ticket after releasing
- Added External/Manual shipping
1.18.1
- Fixed an issue where Driver/Vehicle/Seal were not recorded when shipping with HH
1.17.1
- Added Disclaimer section to Document reports (Receiving slip, Packslip, Proforma, RMA)
- Added ability to override above disclaimer on a Customer/Vendor - Client - Global level
- Added ability to override document logo by Customer/Vendor - Client - Global
- Added ability to delete Carton Sizes even if there were previously assigned to totes
- Added Un-receive RMA handheld function
- Added Non-RMA receiving handheld function
- Added Reason code configurations for above two handheld functions
1.16.2
- Emergency fix for Cycle Count processing
- Emergency fix for PO Receiving double SKU prompt
1.16.1
- Modified how Outstanding Qty to build is calculated. Now (Allocated - Built) instead of (Ordered - Built)
- Added Cycle Count audit report
- Packsize is not prompted on handheld, if only one packsize is available (assumed default)
- Added facility/configuration that prevents LPN to be left on the Floor
- Reworked Analytics menu structure
1.15
- Added cartonization driven picking (both Conventional and Wave picking)
- Added configuration that controls picking behaviour (whether to enforce cartonization or not)
- Added configuration that controls enforcing customer compliance profiles
- Added configuration that controls if carton size/weight should be obeyed during picking
- Modified Waving BigText generation to enable cartonization driven picking
- Added Pallet weight option to BOL/Master BOL
- Added Special Instructions to BOL/Master BOL containing Master/Undelying loads
- Modified Master BOL screen such that Ship to/from could be changed while consolidating
- Added Truck Load reference to Pick Ticket (if multiple, only the first one will be used)
- Added facility to consolidate multiple Truck loads into Master BOL from different Customers
- Added new Tote Merge handheld feature under Fulfillment/Staging
- Added Production consumption styles: Recipe vs WorkArea
1.14
- Added ability to print carton content immediately after carton label
- Extended cartonization, pre-assigning sscc18 code on cartonization
- Added ability to print expected carton content prior to picking based on cartonization results
- Removed Print Sequence as it was replaced with more efficient printing interleaving
- Added Default Freight Terms to customer which will propagate to BOL/Master BOL
- Added Purchase Orders view to a product report
1.13
- Added settings on packsize which controls whether specific packsize is pre packed or not
- Added additional flag on cartonization profile to allow/disallow packsize mixing
- Added default LTL carrier to Customer profile
- Improved grid settings which saves previous sort along with column order and visible columns
- Added reset grid settings to revert back all settings
1.12
- Added Carton sizes
- Added Cartonization profiles to customers
- Added Cartonization functionality
- Added default tote units of measure
- Extended Waving functionality to account for cartonized orders
- Added a way to do Bulk update of Route number on pick tickets
- Added additional settings to Product master which control and supercede cartonization behaviour
- Changed Production bin assignment to show only bins that are not assigned to other WOs
- Added minor UI extensions to highlight over ordered inventory
1.1
- Foreground service fix
- New versioning
1.0.66
- Added facility to import/export LPN inventory
- Added facility to import/export tenant configuration
- Improved Handheld Geo Location service. Location will now update while in sleep
1.0.65
- Added setting to print labels during production
- Added label printing on Production with a separate Template
- Added Pick ticket list to Products screen with Actions
- Added configuration which controls whether Qty/Date values could be scanned
1.0.64
- Added support for Master Bill Of lading management
- Added facility to create User tasks from 'Marked for Cycle Count'
- Added new Cycle Count flavour - 'Cycle count marked bins'
1.0.63
- Added ShipToEmail field to PickTickets and added mask (@ShipToEmail) for Email notifications. For e-comm orders
- Fixed an issue where Lot and Expiry controlled products would not prompt for Expiry properly during Packsize conversion
1.0.62
- Fixed bugs where Cycle count by Product would not work properly with Packsize controlled products
- Added two more additional HH screens for Adjustments (Adj in/out by bin/regular)
1.0.61
- Added pick skipping to Wave and Carton Picking
- Changed LPN Letdown sequencing from being a config to being a separate HH action
- Fixed 'Priority' button on Collaboration/User tasks
- Reworked Non-PO receiving, will now add a PO line to an order with OrderedQty 0
- Enhanced Print product barcodes on PO/Pick tickets to select which SKUs to print
- Added Category visibility on Handheld when scanning SKU
- Added outstanding column to Receiving slip
1.0.60
- Added Product cycle count
- Added ability to Generate cycle count reports
- Added config flag that marks Bins/LPNs for cycle count
- Added marking a bin for Count on Skipping a pick(configurable)
- Added marking a bin for Count on Shorting a pick(configurable)
1.0.59
- Reworked how Cycle count approval works.Now can see deltas during approval.
- Added new config option to control letdown sequence(By SKU/By Bin)
- Added ability to change Freight type while order is in Rating
- Added config which controls whether Pick shorting is allowed to Non admins
- Added config which controls whether Pick skipping is allowed
- Added config which controls Quantity during Carton picking to avoid system freezing and accidental excessive label printing
- Added Line selection in Repackaging when same product from different order lines picked to the same tote
1.0.58
- Added restrictions to prevent allocating/moving product that have pending cycle counts
- Added facility to build Barcoded values for miscellaneous purposes - Barcode notes
1.0.57
- Added Warehouse discontinue feature
- Added additional product refinement for products with same UPC
- Added WO batch picking - can now pick multiple WOs at the same time
- Added Production consumable sourcing.Multiple WOs deplete same bin automatically (Ex: Paint)
1.0.56
- Added Packsize conversion allocation across multiple orders
- Added additional scans to Packsize convert HH screen for more control
- Added LPN Staging to dockdoor
- Added more consolidation options to LPN/Pallets vs Cartons
- Added Tote move options to/from Floor/Bin/Dock
- Added License plate move to Dock
- Added Cycle count approval process
1.0.55
- Added new Carton Pick handheld function
- Added Pick Carton to LPN direct
- Added ability to stage Cartons to LPNs
- Added additional fields to LPNs to record source PO/Container
1.0.54
- Added ability to add/remove lines to BOM on WO
- Created Print Sequence concept for custom compliance label printing
- Added Un-Receive PO handheld function
- Added Non-PO Receiving handhend function
- Added additional reference fields to Inventory locations
- Added Notes to Bins
1.0.53
- Added tagging facility for PickTickets/POs/Returns/WorkOrders/TruckLoads
- Added facility to create notes for PickTickets/POs/Returns/WorkOrders/TruckLoads/Customers/Vendors/Products/Warehouses
- Added authenticated Signing to HandHeld
- Added new allocation styles(Most/Least product first)
- Added configurable 'Auto pick' option
- Added audit logging for User logins/logouts
- Changed how User sessions are tracked, from now on same user cannot login twice
1.0.52
- Background GPS process.Handheld to send GPS location in the background without the need to run app in foreground
- Changed decimal precision on various fields from 2 decimal points to 4
1.0.51
- Working on emitting GPS location silently while app is minimized
1.0.50
- Added Handheld Geo positioning
- Added Order tracking
- Added optional Address verification through Geo coding
- Added Line feed scan to support non Zebra Android handhelds
- Changed Pick ticket email notification email to be sent on Ship instead of Close
1.0.49
- Added import/export of BOL related product details
- Added Back order support for Fulfillment and Receiving
- Added ability to Close Not Received POs
- Added Customer information to RMA report
- Fixed Po list on Handheld for Inbound Transfers
- Removed ability to change Customer/Vendor/Warehouse on SO/PO/RMA for more control
1.0.48
- Added Truck load user/task assignment
- Added Truck Ticket printing
- Optimized Ship handheld screens.Errors are shown before any scanning
- Tote Unstaging process reworked. Totes are fed in sequence and added extra controls
- General fixes
1.0.47
- Added Truck Load processing
- Added Truck load consolidation functionality
- Added Truck load shipping and BOL generation
- Added ability to specify Tote type during Waving (Carton/Pallet/Other)
- Added restrictions to Tote staging based on tote type
- Added Tote to Dock door movement
- Added ability to restrict dock door shipping
- Added Tote staging and dock door movements
- Reworked Handheld and Fulfillment menu structure
- Reworked Configuration structure
1.0.46
- Removed unused configuration settings
- Working on features which allow Warehouse to operate without Web dispatcher
- Added Pickticket list and ability to pick from list
- Added ability to Auto-wave pick tickets from HH
- Added PO list and ability to receive from list
- Added RMA list and ability to receive from list
- Added Work Order list and ability to process from list
1.0.45
- Added ability to modify components on Work Order
- Added easier UI for managing BOM Products
- Added automatic production from Pick tickets
1.0.44
- Added ability to print labels from Custom Actions
- Reworked how Label templates can be sourced with Data Binding
- Added ability to specify printer roles
- Added printer role user matrix
- Added Info(1,2,3...10) fields to PO/PO line/SO/SO line/RMA/RMA line/WO/Product/User
1.0.43
- Improved stability for Internet outages
1.0.42
- Added logout prompt to avoid accidental Logouts
- Added force update of software on startup
- Added clearer messages for WH Transfer processing
- Allowed waving without printing Tote label(for QR scanning)
- Significantly improved PO download performance
1.0.41
- Improved stability of Handheld app
- Further fixes to Messaging on Handheld
- Improved Messaging UI on Web
1.0.40
- Product move regression fix
- Bin move regression fix
1.0.39
- Added avatar upload to User Profile
- Added complete Messaging to both Handheld and Web UI
  - Messages are persisted and could be send via Web/HH
  - Users are notified of new messages in real time
- Removed 'Back' buttons frrom Handheld
  - Native Android back should be used instead
1.0.38
- Fixed Packsize controlled Task assignments
- Fixed Create/Remove of task assignments on bulk operations
- Temporarily removed simple messaging
   - Complete messaging is in development
1.0.37
- Added Task assignments: PickTicket, POs, WOs, Returns
- Added Directed moves: Product move, Full bin move, LPN move
- Added Directed Cycle count
- Added simple messaging to PDT
1.0.36
- Domain change to app.p4warehouse.com
- Dependancy upgrades
- Reworked completely delivery mechanism of on premise workers
1.0.35
- Fixed tote repackage -> HH New tote button
- Fixed tote repackage -> Audit record missing Client
1.0.34
- Added 3PL Billing profiles
- Added more granular 3PL billing setup
- Added 3PL billing wizard
1.0.33
- Adroid 10 and 11 compatibility
- Added Product bundle support
1.0.32
- Reworked Reseller billing model
- Added Customer/Vendor delete
- Added Email certificate for tenant
- Added Seal number for shipping
1.0.31
- Wave picking fixes
1.0.30
- Added Wave picking
- Added License agreement signature requirement
1.0.29
- Domain change to www.p4warehouse.com
- Added Tenant Cloning/Exporting/Importing
1.0.28
- Added 'Tote Repackaging' handheld operation
- Added 'Product locations' handheld menu
- Changed 3PL Client invoice generation to support canonical UoM
- Added Custom Actions entry to PickTicket screen
- Added additional product location logging on Handheld
1.0.27
- Added ability to Enter Bin instead of Scanning
- Added configuration which controls above (Business/Handheld/ForceBinLpnScan)
- Added an ability to use camera for scanning
1.0.26
- Added 'Next tote' toolbar button on Tote Count screen
1.0.25
- Added additional statuses PendingShipCount, PendingDeliveryCount
- Added processes around above new states
- Updated UnShip process
1.0.24
- Added support counting multiple tote lines at once
- Added support for duplicate SKU lines
- Added SerialPattern functionality for SKUs
1.0.23
- Reworked how signatures are captured on Handheld
- Reworked Shipping/Delivery workflow that require signatures
1.0.22
- Added Customer Returns data entry and processing
- Added Customer Returns receiving
- Added Customer Returns photo capture
1.0.21
- Added Substitute conversion functionality
1.0.20
- Added Reference to extended product label print
- Added ability to scan instead of entering numeric values
1.0.19
- Reworked 3PL billing, added support for SQL based line generation
- Added Quarantine zone types.Quarantine zones are non allocatable
- Added UoMs to Product level
- Changed labeling on Handheld to reflect proper UoM
- Added an ability to control whether scan are forced or could be entered manually for:
   - Product/Sku, Po, Pick ticket, Work order
- Reworked Serial Handheld operations
- Added print Tote content label to handheld
1.0.18
- Over-receiving fix for Decimal controlled items
1.0.17
- Decimal control UoM handling fixes
1.0.16
- Modified how Decimal controlled items are being tracked
- Added Length Decimal control
- Added Weight Decimal control
1.0.15
- Added additional Prompt on Cycle count when completing empty bin
- Added support for decoder_i2of5 in default P4W DataWedge Profile
1.0.14
- Added Prompt on Cycle count complete
- Packsize breakdown will continue to next breakdown for that order
- Additional information is displayed with Bin Content option
- Added Packsize breakdown processing for Pending breakdown orders
- Added container receiving
- Localization fixes causing double translations
- Added an ability to print labels during receiving
- Added a standalone menu action to print product labels
- Product labels include UPC, Lot, Expiry, Serial and Packsize barcodes
- Added product image capture
- Fixed localization issues
- Added version consistency with Cloud app".Trim();
    }
}