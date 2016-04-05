#region Usings

using System.Runtime.Serialization;
using DotNetNuke.Services.Authentication.OAuth;

#endregion

namespace DNNAuth.LinkedIn.Components
{
    [DataContract]
    public class LinkedInUserData : UserData
    {
        #region Overrides
		[DataMember(Name = "firstName")]
        public override string FirstName { get; set; }

		[DataMember(Name = "lastName")]
		public override string LastName { get; set; }

		[DataMember(Name = "pictureUrl")]
		public override string ProfileImage { get; set; }
        #endregion
    }
}