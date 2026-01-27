namespace TumorHospital.WebAPI.Documentation
{
    public static class NeedDocs
    {
        #region GetAllNeeds

        public const string GetAllNeedsSummary = "Get all charity needs with pagination";
        public const string GetAllNeedsDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- No authentication is required.<br/>
- This endpoint is publicly accessible.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves a paginated list of all charity needs available in the system.<br/>
- Used to display charity needs to users such as visitors, donors, or volunteers.<br/>
- Supports browsing charity needs with basic details and images.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Query Parameters:</b><br/>
- <b>pageNumber</b> (int, required):<br/>
  - The page number for pagination (starts from 1).<br/><br/>

<b>Pagination:</b><br/>
- Page size is fixed at <b>10 charity needs per page</b>.<br/>
- Results are returned in a paginated response object containing metadata such as total count and total pages.<br/><br/>

<b>Returned Need Data:</b><br/>
Each need item includes:
<ul>
  <li>Title</li>
  <li>Image URL</li>
  <li>Charity category</li>
  <li>Creation date</li>
</ul><br/>

<b>Image Handling:</b><br/>
- Images are returned as full public URLs using the Supabase storage prefix.<br/>
- Frontend can render images directly without additional processing.<br/><br/>

<b>Success Response (200 OK):</b><br/>
- Returns a paginated list of charity needs.<br/><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Invalid page number.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server-side error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Always send <b>pageNumber</b> when requesting data.<br/>
- Use pagination metadata to build paging controls or infinite scrolling.<br/>
- Display images using the returned full image URL.<br/>
";

        #endregion


        #region GetNeed

        public const string GetNeedSummary = "Get charity need details by ID";
        public const string GetNeedDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- No authentication is required.<br/>
- This endpoint is publicly accessible.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves full details of a specific charity need by its unique identifier.<br/>
- Used to display detailed information about a charity need on a details page.<br/>
- Allows users to understand the need description, funding progress, and status.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Route Parameters:</b><br/>
- <b>id</b> (GUID, required):<br/>
  - The unique identifier of the charity need.<br/><br/>

<b>Returned Need Details:</b><br/>
The response includes:
<ul>
  <li>Title</li>
  <li>Description</li>
  <li>Image URL</li>
  <li>Charity category</li>
  <li>Required amount</li>
  <li>Collected amount</li>
  <li>Completion status</li>
  <li>Creation date</li>
</ul><br/>

<b>Image Handling:</b><br/>
- Image is returned as a full public URL using the Supabase storage prefix.<br/>
- Frontend can render the image directly without additional processing.<br/><br/>

<b>Business Rules:</b><br/>
- The charity need must exist in the system.<br/>
- If the provided ID does not match any existing need, the request fails.<br/><br/>

<b>Success Response (200 OK):</b><br/>
- Returns the full details of the requested charity need.<br/><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>The provided need ID does not exist.</li>
    <li>The provided ID format is invalid.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server-side error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Validate the need ID before sending the request if possible.<br/>
- Display a user-friendly message if the need is not found.<br/>
- Use the collected and required amounts to show funding progress visually.<br/>
";

        #endregion


        #region AddNeed

        public const string AddNeedSummary = "Create a new charity need";
        public const string AddNeedDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Admin</b> role.<br/>
- The token must be sent in the request header as follows:<br/>
<pre>
Authorization: Bearer {JWT_TOKEN}
</pre><br/>

<b>Unauthorized / Forbidden Scenarios:</b><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing.</li>
    <li>JWT token is invalid.</li>
    <li>JWT token is expired.</li>
  </ul>
- <b>403 Forbidden</b>:
  <ul>
    <li>User is authenticated but does not have the Admin role.</li>
  </ul><br/>

<b>Purpose:</b><br/>
- Creates a new charity need in the system.<br/>
- Used by administrators to publish fundraising needs for patients, machines, tools, or other charity purposes.<br/>
- Allows the need to be displayed publicly for donations.<br/><br/>

<b>Request Type:</b><br/>
- HTTP POST<br/><br/>

<b>Request Body:</b><br/>
- <b>NewNeedDto</b> containing the charity need data including title, description, category, amount, and image.<br/><br/>

<b>Validation Rules:</b><br/>
- <b>Title</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 100 characters.</li>
  </ul>
- <b>CharityCategory</b>:
  <ul>
    <li>Required.</li>
    <li>Must be one of: Machine, Patient, Tools, Other.</li>
  </ul>
- <b>NeedAmount</b>:
  <ul>
    <li>Required.</li>
    <li>Must be greater than zero.</li>
  </ul>
- <b>Description</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 1000 characters.</li>
  </ul>
- <b>Image</b>:
  <ul>
    <li>Required.</li>
    <li>Allowed formats: .jpg, .jpeg, .png.</li>
    <li>Maximum size: 1 MB.</li>
  </ul><br/>

<b>Image Handling:</b><br/>
- Image is uploaded to cloud storage using the file service.<br/>
- The stored image path is saved with the charity need.<br/>
- Returned needs will include a full public image URL.<br/><br/>

<b>Business Rules:</b><br/>
- Only administrators are allowed to create new charity needs.<br/>
- The charity need is persisted only after successful image upload.<br/>
- The creation process is completed in a single transaction.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""Message"": ""New Need Created Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Validation errors in the submitted need data.</li>
    <li>Invalid charity category.</li>
    <li>Invalid image format or size.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have permission to create charity needs.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server-side error.</li>
    <li>Image upload failure.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Use multipart/form-data when submitting the request with an image file.<br/>
- Validate image size and format on the client side before submission.<br/>
- Show validation messages returned from the API clearly to the admin user.<br/>
- Display a confirmation message after successful creation.<br/>
";

        #endregion


        #region UpdateNeed

        public const string UpdateNeedSummary = "Update an existing charity need";
        public const string UpdateNeedDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Admin</b> role.<br/>
- The token must be sent in the request header as follows:<br/>
<pre>
Authorization: Bearer {JWT_TOKEN}
</pre><br/>

<b>Unauthorized / Forbidden Scenarios:</b><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing.</li>
    <li>JWT token is invalid.</li>
    <li>JWT token is expired.</li>
  </ul>
- <b>403 Forbidden</b>:
  <ul>
    <li>User is authenticated but does not have the Admin role.</li>
  </ul><br/>

<b>Purpose:</b><br/>
- Updates an existing charity need in the system.<br/>
- Allows administrators to modify need details such as title, description, category, amount, and image.<br/>
- Automatically recalculates completion status based on collected amount.<br/><br/>

<b>Request Type:</b><br/>
- HTTP PUT<br/><br/>

<b>Request Parameters:</b><br/>
- <b>id</b> (Guid, required):<br/>
  <ul>
    <li>The unique identifier of the charity need to be updated.</li>
  </ul><br/>

<b>Request Body:</b><br/>
- <b>UpdateNeedDto</b> sent as <b>multipart/form-data</b>.<br/>
- Contains updated charity need fields including image file.<br/><br/>

<b>Validation Rules:</b><br/>
- <b>Title</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 100 characters.</li>
  </ul>
- <b>CharityCategory</b>:
  <ul>
    <li>Required.</li>
    <li>Must be one of: Machine, Patient, Tools, Other.</li>
  </ul>
- <b>NeedAmount</b>:
  <ul>
    <li>Required.</li>
    <li>Must be greater than zero.</li>
  </ul>
- <b>Description</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 1000 characters.</li>
  </ul>
- <b>Image</b>:
  <ul>
    <li>Required.</li>
    <li>Allowed formats: .jpg, .jpeg, .png.</li>
    <li>Maximum size: 1 MB.</li>
  </ul><br/>

<b>Image Handling:</b><br/>
- The existing image is replaced using the file service.<br/>
- Old image is removed or overwritten depending on storage implementation.<br/>
- The updated image path is saved with the charity need.<br/><br/>

<b>Business Rules:</b><br/>
- The charity need must already exist.<br/>
- Completion status (<b>IsCompleted</b>) is automatically set to true if collected amount is greater than or equal to the required amount.<br/>
- Changes are persisted in a single transaction.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""Message"": ""Need Updated Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Validation errors in the submitted data.</li>
    <li>Invalid charity category.</li>
    <li>Invalid image format or size.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have permission to update charity needs.</li>
  </ul><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>Charity need not found.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
    <li>Image update failure.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Use multipart/form-data when submitting the update request.<br/>
- Always send a new image file, as the image field is required.<br/>
- Validate image size and extension on the client side before submission.<br/>
- Show clear confirmation feedback after successful update.<br/>
";

        #endregion


        #region DeleteNeed

        public const string DeleteNeedSummary = "Delete a charity need";
        public const string DeleteNeedDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Admin</b> role.<br/>
- The token must be sent in the request header as follows:<br/>
<pre>
Authorization: Bearer {JWT_TOKEN}
</pre><br/>

<b>Unauthorized / Forbidden Scenarios:</b><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing.</li>
    <li>JWT token is invalid.</li>
    <li>JWT token is expired.</li>
  </ul>
- <b>403 Forbidden</b>:
  <ul>
    <li>User is authenticated but does not have the Admin role.</li>
  </ul><br/>

<b>Purpose:</b><br/>
- Permanently deletes an existing charity need from the system.<br/>
- Used by administrators to remove invalid, completed, or obsolete charity needs.<br/><br/>

<b>Request Type:</b><br/>
- HTTP DELETE<br/><br/>

<b>Route Parameters:</b><br/>
- <b>id</b> (Guid, required):<br/>
  <ul>
    <li>The unique identifier of the charity need to be deleted.</li>
  </ul><br/>

<b>Validation Rules:</b><br/>
- The provided <b>id</b> must be a valid GUID.<br/>
- The charity need must already exist in the system.<br/><br/>

<b>Business Rules:</b><br/>
- Deletion is allowed only if the charity need exists.<br/>
- If the charity need does not exist, the operation is aborted and an error is returned.<br/>
- The deletion is executed as a single transaction.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""Message"": ""Need Deleted Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Invalid request or business rule violation.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have permission to delete charity needs.</li>
  </ul><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>Charity need not found.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Show a confirmation dialog before deleting a charity need.<br/>
- Refresh the needs list after successful deletion.<br/>
- Display clear error messages when deletion fails.<br/>
";

        #endregion


        #region GetCategoriesOfNeeds

        public const string GetCategoriesOfNeedsSummary = "Get charity need categories";
        public const string GetCategoriesOfNeedsDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- No authentication is required.<br/>
- This endpoint is publicly accessible.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves all available charity need categories supported by the system.<br/>
- Categories are derived directly from the <b>CharityCategory</b> enum.<br/>
- Used to populate dropdowns and filters in the frontend when creating or browsing charity needs.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Route:</b><br/>
- <b>/Categories</b><br/><br/>

<b>Response Body:</b><br/>
- <b>CharityCategoriesDto</b> object containing:
  <ul>
    <li><b>categories</b> (List&lt;string&gt;): List of available charity categories.</li>
  </ul><br/>

<b>Business Rules:</b><br/>
- Categories are static and reflect the current values of the <b>CharityCategory</b> enum.<br/>
- No database access is required.<br/>
- The response order matches the enum declaration order.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""categories"": [
    ""Machine"",
    ""Patient"",
    ""Tools"",
    ""Other""
  ]
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Cache the response on the client side since categories are static.<br/>
- Use these values when submitting category fields to avoid validation errors.<br/>
";

        #endregion
    }
}
