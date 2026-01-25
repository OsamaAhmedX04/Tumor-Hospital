namespace TumorHospital.WebAPI.Documentation
{
    public static class SpecializationDocs
    {
        #region GetSpecializations

        public const string GetSpecializationsSummary = "Get all medical specializations";
        public const string GetSpecializationsDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/>
- A valid access token must be sent in the <b>Authorization</b> header.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves a list of all medical specializations in the system.<br/>
- Used mainly for administrative purposes such as:
  <ul>
    <li>Managing doctors</li>
    <li>Assigning specializations</li>
    <li>Viewing specialization metadata</li>
  </ul><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Request Parameters:</b><br/>
- <b>No parameters required.</b><br/><br/>

<b>Business Rules:</b><br/>
- All specializations are returned without pagination.<br/>
- If a specialization has no description, <b>""N/A""</b> is returned instead.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
[
  {
    ""id"": 1,
    ""name"": ""Oncology"",
    ""description"": ""Cancer treatment specialization"",
    ""createdAt"": ""2025-12-10T14:30:00""
  },
  {
    ""id"": 2,
    ""name"": ""Radiology"",
    ""description"": ""N/A"",
    ""createdAt"": ""2025-12-12T09:15:00""
  }
]
</pre><br/>

<b>Error Responses:</b><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>Missing or invalid JWT token.</li>
  </ul><br/>

- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Admin role.</li>
  </ul><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Specializations retrieved successfully.<br/>
- 401 Unauthorized → Authentication required.<br/>
- 403 Forbidden → Access denied.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- No pagination or filtering is required.<br/>
- The response can be cached on the frontend if needed.<br/>
";

        #endregion


        #region GetSpecializationNames

        public const string GetSpecializationNamesSummary = "Get all specialization names";
        public const string GetSpecializationNamesDescription =
            @"
<b>Authentication:</b><br/>
- <b>No authentication required.</b><br/>
- This endpoint is <b>public</b> and can be accessed without a JWT token.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves a lightweight list of all medical specialization names.<br/>
- Optimized for dropdowns, filters, and autocomplete inputs on the frontend.<br/>
- Uses in-memory caching to improve performance and reduce database calls.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Request Parameters:</b><br/>
- <b>No parameters required.</b><br/><br/>

<b>Caching Behavior:</b><br/>
- Data is cached in memory under the key <b>""SpecializationNames""</b>.<br/>
- Cache expiration: <b>3 days</b> (absolute expiration).<br/>
- If cache exists, data is returned directly without querying the database.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
[
  ""Oncology"",
  ""Radiology"",
  ""Cardiology"",
  ""Neurology""
]
</pre><br/>

<b>Error Responses:</b><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Specialization names retrieved successfully.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Ideal for dropdowns and filters.<br/>
- Cache-friendly endpoint; safe to call frequently.<br/>
- No pagination is needed since the response is a simple string list.<br/>
";

        #endregion


        #region AddSpecializations

        public const string AddSpecializationsSummary = "Create a new medical specialization";
        public const string AddSpecializationsDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/><br/>

<b>Purpose:</b><br/>
- Adds a new medical specialization to the system.<br/>
- Clears the cached list of specialization names after creation to ensure data consistency.<br/><br/>

<b>Request Type:</b><br/>
- HTTP POST<br/><br/>

<b>Request Body:</b><br/>
- <b>Content-Type:</b> application/json<br/>
- <b>Body Example:</b>
<pre>
{
  ""name"": ""Cardiology"",
  ""description"": ""Heart-related specialization""
}
</pre><br/>

<b>Validation Rules:</b><br/>
- <b>name</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 50 characters.</li>
    <li>Must be unique (case-insensitive).</li>
  </ul>
- <b>description</b>:
  <ul>
    <li>Optional.</li>
    <li>If not provided, defaults to 'N/A'.</li>
  </ul><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Specialization Created Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Specialization name is empty.</li>
    <li>Specialization name exceeds 50 characters.</li>
    <li>Specialization with the same name already exists.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error during creation.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Ensure the name is unique before calling this endpoint to avoid errors.<br/>
- Description is optional and can be left blank.<br/>
- After creation, the frontend should refresh any cached dropdowns of specialization names.<br/>
";

        #endregion


        #region UpdateSpecializations

        public const string UpdateSpecializationsSummary = "Update an existing medical specialization";
        public const string UpdateSpecializationsDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/><br/>

<b>Purpose:</b><br/>
- Updates the name and description of an existing medical specialization.<br/>
- Clears the cached list of specialization names after update to ensure consistency.<br/><br/>

<b>Request Type:</b><br/>
- HTTP PUT<br/><br/>

<b>Route Parameters:</b><br/>
- <b>id</b> (Guid, required):<br/>
  - The unique identifier of the specialization to update.<br/><br/>

<b>Request Body:</b><br/>
- <b>Content-Type:</b> application/json<br/>
- <b>Body Example:</b>
<pre>
{
  ""name"": ""Cardiology"",
  ""description"": ""Heart-related specialization""
}
</pre><br/>

<b>Validation Rules:</b><br/>
- <b>name</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 50 characters.</li>
    <li>Must be unique (case-insensitive).</li>
  </ul>
- <b>description</b>:
  <ul>
    <li>Optional.</li>
    <li>If not provided, defaults to 'N/A'.</li>
  </ul><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Specialization Updated Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Specialization name is empty.</li>
    <li>Specialization name exceeds 50 characters.</li>
    <li>Specialization with the same name already exists.</li>
  </ul><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>Specialization with the provided id does not exist.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error during update.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Ensure the name is unique before updating to avoid conflicts.<br/>
- Description can be left blank; it will default to 'N/A'.<br/>
- After update, refresh any cached dropdowns or lists containing specialization names.<br/>
";

        #endregion


        #region DeleteSpecializations

        public const string DeleteSpecializationsSummary = "Delete an existing medical specialization";
        public const string DeleteSpecializationsDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b< br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/><br/>

<b>Purpose:</b><br/>
- Deletes a medical specialization by its unique identifier.<br/>
- Clears the cached list of specialization names after deletion to maintain consistency.<br/><br/>

<b>Request Type:</b><br/>
- HTTP DELETE<br/><br/>

<b>Route Parameters:</b><br/>
- <b>id</b> (Guid, required):<br/>
  - The unique identifier of the specialization to delete.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Specialization Deleted Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>Specialization with the provided id does not exist.</li>
  </ul><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Invalid request parameters.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error during deletion.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Ensure no dependencies (like doctors linked to this specialization) before deletion.<br/>
- After deletion, refresh any cached dropdowns or lists containing specialization names.<br/>
";

        #endregion
    }
}
