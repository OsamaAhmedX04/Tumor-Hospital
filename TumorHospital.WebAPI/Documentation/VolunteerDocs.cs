using System.Dynamic;

namespace TumorHospital.WebAPI.Documentation
{
    public static class VolunteerDocs
    {
        #region GetAllVolunteers

        public const string GetAllVolunteersSummary = "Retrieve a paginated list of all volunteers";
        public const string GetAllVolunteersDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Accessible only by users with <b>Admin</b> or <b>Receptionist</b> roles.<br/><br/>

<b>Purpose:</b><br/>
- Returns a paginated list of volunteers who have made donations.<br/>
- Includes volunteer personal information and donation details.<br/><br/>

<b>Query Parameters:</b><br/>
- <b>pageNumber</b> (int, required): Page number to retrieve.<br/><br/>

<b>Response Fields:</b><br/>
- volunteerName: Name of the volunteer.<br/>
- email: Volunteer email (if provided).<br/>
- phone: Volunteer phone number (if provided).<br/>
- amountDonated: Amount donated by the volunteer.<br/>
- charityNeedCategory: Category of the charity need associated with the donation.<br/>
- donationDate: Date of the donation.<br/><br/>

<b>Success Response (200 OK):</b><br/>
Returns paginated volunteer data.<br/><br/>

<b>Error Responses:</b><br/>
- 401 Unauthorized: JWT token missing or invalid.<br/>
- 403 Forbidden: User does not have the required role.<br/>
- 500 Internal Server Error: Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Use pageNumber to implement pagination.<br/>
- Display volunteer donation details in a table or list.<br/>
";

        #endregion
    }
}
