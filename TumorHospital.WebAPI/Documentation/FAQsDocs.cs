namespace TumorHospital.WebAPI.Documentation
{
    public static class FAQsDocs
    {
        #region GetAllFAQs

        public const string GetAllFAQsSummary = "Retrieve all frequently asked questions (FAQs)";
        public const string GetAllFAQsDescription =
            @"
<b>Authentication:</b><br/>
- No authentication required.<br/>
- Publicly accessible endpoint.<br/><br/>

<b>Purpose:</b><br/>
- Returns a list of all Frequently Asked Questions (FAQs).<br/>
- Used to provide common questions and answers to users.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Response Fields:</b><br/>
- id: Unique identifier of the FAQ.<br/>
- question: The frequently asked question text.<br/>
- answer: The answer related to the question.<br/><br/>

<b>Success Response (200 OK):</b><br/>
Returns a list of FAQs.<br/><br/>

<b>Error Responses:</b><br/>
- 500 Internal Server Error: Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Display FAQs in an accordion or expandable list for better UX.<br/>
- This endpoint can be cached since FAQs do not change frequently.<br/>
";
        #endregion


        #region AddFAQ

        public const string AddFAQSummary = "Add a new FAQ";
        public const string AddFAQDescription =
            @"
<b>Authentication:</b><br/>
- JWT authentication required.<br/>
- Accessible only by users with the <b>Admin</b> role.<br/><br/>

<b>Purpose:</b><br/>
- Adds a new Frequently Asked Question (FAQ) to the system.<br/>
- Used by admins to manage and enrich the FAQ section displayed to users.<br/><br/>

<b>Request Type:</b><br/>
- HTTP POST<br/><br/>

<b>Request Body:</b><br/>
- <b>NewFAQsDto</b> object containing:
  <ul>
    <li><b>question</b> (string, required)</li>
    <li><b>answer</b> (string, required)</li>
  </ul><br/>

<b>Validation Rules:</b><br/>
- <b>question</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: <b>500 characters</b>.</li>
  </ul><br/>

- <b>answer</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: <b>2000 characters</b>.</li>
  </ul><br/>

<b>Business Rules:</b><br/>
- Only admins are allowed to add new FAQs.<br/>
- The FAQ is saved immediately after successful validation.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""FAQ added successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Validation errors (empty or too long question/answer).</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Admin role.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Validate input lengths before sending the request to improve UX.<br/>
- Display validation errors returned from the API directly to the admin user.<br/>
";
        #endregion


        #region UpdateFAQ

        public const string UpdateFAQSummary = "Update an existing FAQ";
        public const string UpdateFAQDescription =
            @"
<b>Authentication:</b><br/>
- JWT authentication required.<br/>
- Accessible only by users with the <b>Admin</b> role.<br/><br/>

<b>Purpose:</b><br/>
- Updates the question and answer of an existing Frequently Asked Question (FAQ).<br/>
- Used by admins to modify outdated or incorrect FAQ content.<br/><br/>

<b>Request Type:</b><br/>
- HTTP PUT<br/><br/>

<b>Route Parameters:</b><br/>
- <b>id</b> (int, required):<br/>
  - The unique identifier of the FAQ to be updated.<br/><br/>

<b>Request Body:</b><br/>
- <b>NewFAQsDto</b> object containing:
  <ul>
    <li><b>question</b> (string, required)</li>
    <li><b>answer</b> (string, required)</li>
  </ul><br/>

<b>Validation Rules:</b><br/>
- <b>question</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: <b>500 characters</b>.</li>
  </ul><br/>

- <b>answer</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: <b>2000 characters</b>.</li>
  </ul><br/>

<b>Business Rules:</b><br/>
- Only admins are allowed to update FAQs.<br/>
- The FAQ must already exist; otherwise, an error is returned.<br/>
- Only the question and answer fields are updated.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""FAQ updated successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Validation errors (empty or too long question/answer).</li>
  </ul><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>FAQ not found.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Admin role.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Pre-fill the form with the current FAQ data before updating.<br/>
- Prevent submitting empty fields on the client side.<br/>
- Display validation and not-found errors returned from the API clearly to the admin user.<br/>
";
        #endregion


        #region DeleteFAQ

        public const string DeleteFAQSummary = "Delete an existing FAQ";
        public const string DeleteFAQDescription =
            @"
<b>Authentication:</b><br/>
- JWT authentication required.<br/>
- Accessible only by users with the <b>Admin</b> role.<br/><br/>

<b>Purpose:</b><br/>
- Deletes an existing Frequently Asked Question (FAQ) permanently from the system.<br/>
- Used by admins to remove outdated, duplicated, or irrelevant FAQs.<br/><br/>

<b>Request Type:</b><br/>
- HTTP DELETE<br/><br/>

<b>Route Parameters:</b><br/>
- <b>id</b> (int, required):<br/>
  - The unique identifier of the FAQ to be deleted.<br/><br/>

<b>Business Rules:</b><br/>
- Only admins are allowed to delete FAQs.<br/>
- The FAQ must already exist; otherwise, an error is returned.<br/>
- Deletion is permanent and cannot be undone.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""FAQ deleted successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>FAQ not found.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Admin role.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Always show a confirmation dialog before deleting an FAQ.<br/>
- After successful deletion, refresh the FAQ list immediately.<br/>
- Display server error messages clearly if deletion fails.<br/>
";
        #endregion
    }
}
