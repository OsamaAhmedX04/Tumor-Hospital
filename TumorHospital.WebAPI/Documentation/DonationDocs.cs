namespace TumorHospital.WebAPI.Documentation
{
    public static class DonationDocs
    {
        #region Donate

        public const string DonateSummary = "Add a volunteer donation to a charity need";
        public const string DonateDescription =
            @"
<b>Authentication:</b><br/>
- <b>No authentication required.</b> Public endpoint.<br/><br/>

<b>Purpose:</b><br/>
- Allows a volunteer to donate to a specific charity need.<br/>
- Saves volunteer details and updates the collected amount.<br/><br/>

<b>Request Body:</b><br/>
- <b>VolunteerDto</b> object containing:
  <ul>
    <li>volunteerName (string, required)</li>
    <li>email (string, optional)</li>
    <li>phone (string, optional)</li>
    <li>amountDonated (decimal, required)</li>
    <li>charityNeedId (Guid, required)</li>
  </ul><br/>

<b>Validation Rules:</b><br/>
- volunteerName: required, max 100 characters.<br/>
- email: optional, must be valid format if provided.<br/>
- phone: optional, must match international format if provided.<br/>
- amountDonated: required, greater than 0.<br/>
- charityNeedId: required, must reference existing need.<br/><br/>

<b>Business Rules:</b><br/>
- Charity need must exist.<br/>
- Donation added to collected amount.<br/><br/>

<b>Success Response:</b><br/>
200 OK with message 'Donation Added Successfully'.<br/><br/>

<b>Error Responses:</b><br/>
- 400 Bad Request: validation errors or invalid charityNeedId.<br/>
- 500 Internal Server Error: unexpected server error.<br/>
";

        #endregion
    }
}
