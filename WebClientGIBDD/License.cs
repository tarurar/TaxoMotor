//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebClientGIBDD
{
    using System;
    using System.Collections.Generic;
    
    public partial class License
    {
        public License()
        {
            this.License1 = new HashSet<License>();
            this.License11 = new HashSet<License>();
            this.SpecialVehiclesRegister = new HashSet<SpecialVehiclesRegister>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string RegNumber { get; set; }
        public Nullable<int> RegNumberInt { get; set; }
        public string BlankSeries { get; set; }
        public string BlankNo { get; set; }
        public string OrgName { get; set; }
        public string Ogrn { get; set; }
        public string Inn { get; set; }
        public Nullable<int> Parent { get; set; }
        public Nullable<int> RootParent { get; set; }
        public Nullable<int> Status { get; set; }
        public string Document { get; set; }
        public string Signature { get; set; }
        public Nullable<int> TaxiId { get; set; }
        public string Lfb { get; set; }
        public string JuridicalAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string AddContactData { get; set; }
        public string AccountAbbr { get; set; }
        public string TaxiBrand { get; set; }
        public string TaxiModel { get; set; }
        public string TaxiStateNumber { get; set; }
        public Nullable<int> TaxiYear { get; set; }
        public Nullable<System.DateTime> OutputDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> TillDate { get; set; }
        public Nullable<System.DateTime> TillSuspensionDate { get; set; }
        public string CancellationReason { get; set; }
        public string SuspensionReason { get; set; }
        public string ChangeReason { get; set; }
        public string InvalidReason { get; set; }
        public string ShortName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public Nullable<System.DateTime> OgrnDate { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Building { get; set; }
        public string Structure { get; set; }
        public string Facility { get; set; }
        public string Ownership { get; set; }
        public string Flat { get; set; }
        public string Fax { get; set; }
        public string EMail { get; set; }
        public string TaxiColor { get; set; }
        public string TaxiNumberColor { get; set; }
        public string TaxiVin { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public string Guid_OD { get; set; }
        public Nullable<System.DateTime> Date_OD { get; set; }
        public Nullable<bool> FromPortal { get; set; }
        public string FirmName { get; set; }
        public string Brand { get; set; }
        public string OgrnNum { get; set; }
        public string OgrnName { get; set; }
        public string GRAddress { get; set; }
        public Nullable<System.DateTime> InnDate { get; set; }
        public string InnName { get; set; }
        public string InnNum { get; set; }
        public string Address_Fact { get; set; }
        public string Country_Fact { get; set; }
        public string PostalCode_Fact { get; set; }
        public string Locality_Fact { get; set; }
        public string Region_Fact { get; set; }
        public string City_Fact { get; set; }
        public string Town_Fact { get; set; }
        public string Street_Fact { get; set; }
        public string House_Fact { get; set; }
        public string Building_Fact { get; set; }
        public string Structure_Fact { get; set; }
        public string Facility_Fact { get; set; }
        public string Ownership_Fact { get; set; }
        public string Flat_Fact { get; set; }
        public Nullable<bool> Gps { get; set; }
        public Nullable<bool> Taxometr { get; set; }
        public Nullable<System.DateTime> TODate { get; set; }
        public string STSNumber { get; set; }
        public Nullable<System.DateTime> STSDate { get; set; }
        public Nullable<int> OwnType { get; set; }
        public string OwnNumber { get; set; }
        public Nullable<System.DateTime> OwnDate { get; set; }
        public Nullable<bool> MO { get; set; }
        public string GUID_MO { get; set; }
        public Nullable<System.DateTime> DATE_MO { get; set; }
        public Nullable<bool> Obsolete { get; set; }
        public Nullable<bool> DisableGibddSend { get; set; }
    
        public virtual ICollection<License> License1 { get; set; }
        public virtual License License2 { get; set; }
        public virtual ICollection<License> License11 { get; set; }
        public virtual License License3 { get; set; }
        public virtual ICollection<SpecialVehiclesRegister> SpecialVehiclesRegister { get; set; }
    }
}
