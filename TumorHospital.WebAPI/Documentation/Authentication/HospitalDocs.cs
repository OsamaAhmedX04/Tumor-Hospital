namespace TumorHospital.WebAPI.Documentation.Authentication
{
    public static class HospitalDocs
    {
        #region GetAllHospitals
        public const string GetAllHospitalsSummary = "Get all hospitals basic information";
        public const string GetAllHospitalsDescription =
            @"
<b>Purpose:</b><br/>
- Retrieves a list of all hospitals in the system.<br/>
- Returns basic and aggregated information for each hospital.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>JWT token is required.</b><br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/><br/>

<b>How to send the JWT:</b><br/>
- Add the token to the <b>Authorization</b> header like this:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Who can access:</b><br/>
- <b>Admin</b> users only.<br/><br/>

<b>Request Parameters:</b><br/>
- None.<br/><br/>

<b>Returned Data Per Hospital:</b><br/>
- Hospital ID<br/>
- Hospital Name<br/>
- Government (Governorate)<br/>
- Address<br/>

<b>Business Logic:</b><br/>
1. Fetch all hospitals from database.<br/>
2. Project hospital entities into <b>HospitalInfoDto</b>.<br/>
4. Sort hospitals alphabetically by name.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
[
  {
    ""id"": 1,
    ""name"": ""Cairo Oncology Hospital"",
    ""government"": ""Cairo"",
    ""address"": ""Nasr City"",
  }
]
</pre><br/>

<b>Authentication & Authorization Errors:</b><br/>
- <b>401 Unauthorized</b>:<br/>
  - JWT token missing.<br/>
  - JWT token invalid or expired.<br/><br/>
- <b>403 Forbidden</b>:<br/>
  - JWT token valid but user role is not Admin.<br/><br/>

<b>Other Errors:</b><br/>
- <b>500 Internal Server Error</b>:<br/>
  - Unexpected error while retrieving hospitals data.<br/><br/>

<b>Notes for Frontend:</b><br/>
- Include Admin JWT token in the header.<br/>
- Data is read-only.<br/>
- Endpoint returns all hospitals in alphabetical order.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Hospitals retrieved successfully.<br/>
- 401 Unauthorized → Missing / invalid / expired JWT.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region GetHospitalDashboard
        public const string GetHospitalDashboardSummary = "Get hospital dashboard statistics";
        public const string GetHospitalDashboardDescription =
            @"
<b>Purpose:</b><br/>
- Retrieves dashboard statistics for a specific hospital.<br/>
- Used by admin to monitor hospital capacity and current staff numbers.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>This endpoint is protected.</b><br/>
- You <b>must</b> send a valid JWT token in the request header.<br/><br/>

<b>How to send the JWT:</b><br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Who can access:</b><br/>
- Only users with <b>Admin</b> role.<br/><br/>

<b>Request Parameters:</b><br/>
- <b>hospitalId</b> (GUID, required): The ID of the hospital whose dashboard data is required.<br/><br/>

<b>Business Logic:</b><br/>
1. Check if the hospital exists using the provided <b>hospitalId</b>.<br/>
2. If found, calculate current numbers of doctors and receptionists.<br/>
3. Return hospital capacity limits and current usage.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""maxNumberOfDoctors"": 50,
    ""maxNumberOfReceptionists"": 20,
    ""numberOfDoctors"": 35,
    ""numberOfReceptionists"": 12
}
</pre><br/>

<b>Business Errors (400 Bad Request):</b><br/>
- <b>Hospital Not Exist</b>: No hospital found with the given ID.<br/><br/>

<b>Authentication Errors:</b><br/>
- <b>401 Unauthorized</b>:<br/>
  - JWT token is missing.<br/>
  - JWT token is invalid or expired.<br/><br/>

- <b>403 Forbidden</b>:<br/>
  - JWT token is valid but user role is not <b>Admin</b>.<br/><br/>

<b>Other Errors:</b><br/>
- <b>500 Internal Server Error</b>: Unexpected server error.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Dashboard data retrieved successfully.<br/>
- 400 Bad Request → Hospital does not exist.<br/>
- 401 Unauthorized → Missing / invalid / expired JWT.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region GetAllHospitalDoctors
        public const string GetAllHospitalDoctorsSummary = "Get paginated list of doctors in a specific hospital";
        public const string GetAllHospitalDoctorsDescription =
            @"
<b>Purpose:</b><br/>
- Retrieves a paginated list of all doctors working in a specific hospital.<br/>
- Supports searching doctors by full name.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>JWT token required</b> in the request header.<br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/><br/>

<b>How to send the JWT:</b><br/>
- Add the token to the <b>Authorization</b> header:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Request Parameters:</b><br/>
- <b>hospitalId</b> (GUID, required) → The ID of the hospital.<br/>
- <b>doctorName</b> (string, optional) → Filter doctors by full name (first + last name).<br/>
- <b>pageNumber</b> (int, optional, default = 1) → Pagination page number.<br/><br/>

<b>Business Logic:</b><br/>
1. Fetch all doctors for the given hospitalId.<br/>
2. If <b>doctorName</b> is provided, filter doctors whose full name contains the search term.<br/>
3. Paginate the results, 10 doctors per page.<br/>
4. Map each doctor to <b>DoctorDto</b> including profile image URL.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""pageNumber"": 1,
  ""pageSize"": 10,
  ""totalPages"": 5,
  ""totalItems"": 47,
  ""items"": [
    {
      ""id"": ""guid-of-doctor"",
      ""fullName"": ""Dr. Ahmed Ali"",
      ""gender"": ""Male"",
      ""profileImageUrl"": ""https://supabaseurl/.../profile.jpg""
    }
  ]
}
</pre><br/>

<b>Authentication & Authorization Errors:</b><br/>
- <b>401 Unauthorized</b> → JWT missing, invalid, or expired.<br/>
- <b>403 Forbidden</b> → JWT valid but user role is not Admin.<br/><br/>

<b>Other Errors (400 Bad Request):</b><br/>
- Invalid <b>hospitalId</b>.<br/>
- Unexpected server errors while fetching data.<br/><br/>

<b>Notes for Frontend:</b><br/>
- Profile images URLs are prefixed with Supabase URL.<br/>
- Pagination is 10 items per page.<br/>
- Use <b>doctorName</b> query parameter to implement search functionality.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Doctors retrieved successfully.<br/>
- 400 Bad Request → Invalid hospital ID or server error.<br/>
- 401 Unauthorized → Missing / invalid / expired JWT.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region GetHospitalDoctor
        public const string GetHospitalDoctorSummary = "Get detailed information of a specific doctor";
        public const string GetHospitalDoctorDescription =
            @"
<b>Purpose:</b><br/>
- Retrieves detailed information for a single doctor.<br/>
- Includes personal info, specialization, costs, surgeon status, and working schedule.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>JWT token is required</b> in the request header.<br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/><br/>

<b>How to send the JWT:</b><br/>
- Add the token to the <b>Authorization</b> header:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Who can access:</b><br/>
- <b>Admin</b> users only.<br/><br/>

<b>Request Parameters:</b><br/>
- <b>doctorId</b> (string, required) → The ID of the doctor.<br/><br/>

<b>Business Logic:</b><br/>
1. Fetch doctor by <b>doctorId</b> where the doctor is active.<br/>
2. Map to <b>DoctorInformationDto</b> including:<br/>
   - Full Name<br/>
   - Gender<br/>
   - Profile Image URL (prefixed with Supabase URL if exists)<br/>
   - Bio<br/>
   - Specialization Name<br/>
   - IsSurgeon flag<br/>
   - ConsultationCost, FollowUpCost, SurgeryCost (if surgeon)<br/>
   - WorkingDays schedule<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""id"": ""guid-of-doctor"",
  ""fullName"": ""Dr. Ahmed Ali"",
  ""profileImageUrl"": ""https://supabaseurl/.../profile.jpg"",
  ""gender"": ""Male"",
  ""bio"": ""Cardiology specialist with 10 years experience"",
  ""specialization"": ""Cardiology"",
  ""isSurgeon"": true,
  ""consultationCost"": 200,
  ""followUpCost"": 100,
  ""surgeryCost"": 5000,
  ""workingDays"": [
    { ""day"": ""Monday"", ""fromTime"": ""08:00"", ""toTime"": ""14:00"" },
    { ""day"": ""Wednesday"", ""fromTime"": ""10:00"", ""toTime"": ""16:00"" }
  ]
}
</pre><br/>

<b>Authentication & Authorization Errors:</b><br/>
- <b>401 Unauthorized</b> → JWT missing, invalid, or expired.<br/>
- <b>403 Forbidden</b> → JWT valid but user role is not Admin.<br/><br/>

<b>Other Errors (400 Bad Request):</b><br/>
- Doctor not found or inactive.<br/>
- Unexpected server errors.<br/><br/>

<b>Notes for Frontend:</b><br/>
- ProfileImageUrl may be null if doctor has no profile picture.<br/>
- SurgeryCost is null if doctor is not a surgeon.<br/>
- WorkingDays includes day of the week and start/end times.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Doctor info retrieved successfully.<br/>
- 400 Bad Request → Doctor not found or other errors.<br/>
- 401 Unauthorized → Missing / invalid / expired JWT.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region GetAllHospitalReceptionists
        public const string GetAllHospitalReceptionistsSummary = "Get paginated list of receptionists for a specific hospital";
        public const string GetAllHospitalReceptionistsDescription =
            @"
<b>Purpose:</b><br/>
- Retrieves a paginated list of receptionists for a given hospital.<br/>
- Includes basic info like full name and ID.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>JWT token is required</b> in the request header.<br/>
- Only users with the <b>Admin</b> role can access this endpoint.<br/><br/>

<b>How to send the JWT:</b><br/>
- Add the token to the <b>Authorization</b> header:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Who can access:</b><br/>
- <b>Admin</b> users only.<br/><br/>

<b>Request Parameters:</b><br/>
- <b>hospitalId</b> (Guid, required) → The ID of the hospital.<br/>
- <b>receptionistName</b> (string, optional) → Filter receptionists by full name.<br/>
- <b>pageNumber</b> (int, optional, default 1) → The page number for pagination.<br/><br/>

<b>Business Logic:</b><br/>
1. Fetch receptionists associated with the specified hospital.<br/>
2. Apply name filter if provided.<br/>
3. Paginate results (page size = 10).<br/>
4. Map to <b>ReceptionistDto</b> (Id, Name).<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""data"": [
    { ""id"": ""guid-of-receptionist"", ""name"": ""Sara Ahmed"" },
    { ""id"": ""guid-of-receptionist"", ""name"": ""Mona Ali"" }
  ],
  ""currentPage"": 1,
  ""totalPages"": 5,
  ""totalCount"": 50
}
</pre><br/>

<b>Authentication & Authorization Errors:</b><br/>
- <b>401 Unauthorized</b> → JWT missing, invalid, or expired.<br/>
- <b>403 Forbidden</b> → JWT valid but user role is not Admin.<br/><br/>

<b>Other Errors (400 Bad Request):</b><br/>
- Hospital not found.<br/>
- Unexpected server errors.<br/><br/>

<b>Notes for Frontend:</b><br/>
- Pagination info included: currentPage, totalPages, totalCount.<br/>
- Name filter is optional and case-insensitive.<br/>
- Each receptionist object contains only Id and Name for simplicity.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Receptionists retrieved successfully.<br/>
- 400 Bad Request → Hospital not found or other errors.<br/>
- 401 Unauthorized → Missing / invalid / expired JWT.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region GetHospitalGovernmentsExistance
        public const string GetHospitalGovernmentsExistanceSummary = "Get list of all hospital governments";
        public const string GetHospitalGovernmentsExistanceDescription =
            @"
<b>Purpose:</b><br/>
- Retrieves a list of all governments where hospitals exist.<br/>
- Used for filtering hospitals by location (government), dropdowns, and search forms in the frontend.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>No authentication required.</b><br/>
- This endpoint is <b>public</b> and does not require a JWT token.<br/><br/>

<b>Request Parameters:</b><br/>
- None.<br/><br/>

<b>Business Logic:</b><br/>
1. Check if hospital governments exist in the in-memory cache (<b>HospitalsGovernments</b>).<br/>
2. If cached data exists, return it immediately.<br/>
3. If not cached:<br/>
   - Fetch hospital governments from the database.<br/>
   - Store them in memory cache for <b>3 days</b>.<br/>
4. Return the list of governments.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
[
    ""Cairo"",
    ""Giza"",
    ""Alexandria"",
    ""Dakahlia""
]
</pre><br/>

<b>Empty Response Case:</b><br/>
- If no hospitals exist, an empty array will be returned:<br/>
<pre>
[]
</pre><br/><br/>

<b>Performance Notes:</b><br/>
- Uses <b>in-memory caching</b> to reduce database load.<br/>
- Cache duration: <b>3 days</b>.<br/>
- Safe to be called frequently by the frontend.<br/><br/>

<b>Errors:</b><br/>
- <b>500 Internal Server Error</b>: Unexpected server error while fetching governments.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Governments retrieved successfully.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region GetHospitalsNames
        public const string GetHospitalsNamesSummary = "Get all hospital names";
        public const string GetHospitalsNamesDescription =
            @"
<b>Purpose:</b><br/>
- Retrieves a list of all hospital names only.<br/>
- Used mainly for dropdowns, filters, search inputs, and autocomplete fields in the frontend.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>No authentication required.</b><br/>
- This endpoint is <b>public</b> and does not require a JWT token.<br/><br/>

<b>Request Parameters:</b><br/>
- None.<br/><br/>

<b>Business Logic:</b><br/>
1. Check if hospital names exist in the in-memory cache (<b>HospitalsNames</b>).<br/>
2. If cached data exists, return it directly (fast response).<br/>
3. If not cached:<br/>
   - Fetch hospital names from the database.<br/>
   - Store them in memory cache for <b>3 days</b>.<br/>
4. Return the list of hospital names.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
[
    ""Cairo Oncology Hospital"",
    ""Alexandria Cancer Center"",
    ""Giza Medical Institute""
]
</pre><br/>

<b>Empty Response Case:</b><br/>
- If no hospitals exist, an empty array will be returned:<br/>
<pre>
[]
</pre><br/>

<b>Performance Notes:</b><br/>
- Uses <b>in-memory caching</b> to reduce database load.<br/>
- Cache duration: <b>3 days</b>.<br/>
- Ideal for frequent frontend calls.<br/><br/>

<b>Errors:</b><br/>
- <b>500 Internal Server Error</b>: Unexpected server error while fetching hospital names.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Hospital names retrieved successfully.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region AddHospital
        public const string AddHospitalSummary = "Add a new hospital";
        public const string AddHospitalDescription =
            @"
<b>Purpose:</b><br/>
- Adds a new hospital to the system.<br/>
- Stores information including name, government, address, and maximum number of doctors & receptionists.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>This endpoint is protected.</b><br/>
- You <b>must</b> send a valid JWT token in the request header.<br/><br/>

<b>How to send the JWT:</b><br/>
- Add the token to the <b>Authorization</b> header like this:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Who can access:</b><br/>
- Only users with <b>Admin</b> role.<br/><br/>

<b>Request Parameters:</b><br/>
- <b>Body (HospitalDto)</b> contains:<br/>
  - <b>Name</b> (string, required, max 100 chars): Name of the hospital.<br/>
  - <b>Government</b> (string, required, max 100 chars): Government/region of the hospital.<br/>
  - <b>Address</b> (string, required, max 300 chars): Full address of the hospital.<br/>
  - <b>MaxNumberOfDoctors</b> (int, required, 1-200): Maximum allowed doctors.<br/>
  - <b>MaxNumberOfReceptionists</b> (int, required, 1-200): Maximum allowed receptionists.<br/><br/>

<b>Business Logic:</b><br/>
1. Validate input using FluentValidation.<br/>
2. Check for duplicate hospital <b>Name</b> or <b>Address</b>.<br/>
3. Map DTO to entity and save to database.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""New Hospital Added Successfully""
}
</pre><br/>

<b>Validation Errors (400 Bad Request):</b><br/>
- Returned if any required field is missing or invalid.<br/>
- Example:<br/>
<pre>
{
    ""Errors"": {
        ""Name"": [""Hospital name is required.""],
        ""Address"": [""Address must not exceed 300 characters.""]
    }
}
</pre><br/>

<b>Business Errors (400 Bad Request):</b><br/>
- <b>Name or Address Already Exists</b>:<br/>
  - Response message: ""This Name Already Exist"" or ""This Address Already Exist""<br/><br/>

<b>Authentication Errors:</b><br/>
- <b>401 Unauthorized</b>: Missing or invalid JWT token.<br/>
- <b>403 Forbidden</b>: User role is not Admin.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Hospital added successfully.<br/>
- 400 Bad Request → Validation or business errors.<br/>
- 401 Unauthorized → Missing or invalid JWT.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region UpdateHospital
        public const string UpdateHospitalSummary = "Update an existing hospital";
        public const string UpdateHospitalDescription =
            @"
<b>Purpose:</b><br/>
- Updates the information of an existing hospital.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>This endpoint is protected.</b><br/>
- Must send a valid JWT token in the <b>Authorization</b> header.<br/><br/>

<b>Who can access:</b><br/>
- Only users with <b>Admin</b> role.<br/><br/>

<b>Request Parameters:</b><br/>
- <b>hospitalId</b> (GUID, required): The ID of the hospital to update.<br/>
- <b>Body (HospitalDto)</b> contains:<br/>
  - <b>Name</b> (string, required, max 100 chars): Name of the hospital.<br/>
  - <b>Government</b> (string, required, max 100 chars): Government/region of the hospital.<br/>
  - <b>Address</b> (string, required, max 300 chars): Full address of the hospital.<br/>
  - <b>MaxNumberOfDoctors</b> (int, required, 1-200): Maximum allowed doctors.<br/>
  - <b>MaxNumberOfReceptionists</b> (int, required, 1-200): Maximum allowed receptionists.<br/><br/>

<b>Business Logic:</b><br/>
1. Validate the request body according to the above rules.<br/>
2. Check if the <b>hospitalId</b> exists.<br/>
3. Check if the <b>Address</b> or <b>Name</b> is duplicated in another hospital.<br/>
4. Update the hospital fields and save changes.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""Hospital Updated Successfully""
}
</pre><br/>

<b>Validation Errors (400 Bad Request):</b><br/>
- Name, Government, Address, MaxNumberOfDoctors, or MaxNumberOfReceptionists fail validation.<br/>
- Duplicate Name or Address.<br/><br/>

<b>Authentication Errors:</b><br/>
- <b>401 Unauthorized</b>: Missing/invalid/expired JWT.<br/>
- <b>403 Forbidden</b>: JWT valid but user role is not Admin.<br/><br/>

<b>Other Errors (500 Internal Server Error):</b><br/>
- Unexpected database or server error.<br/><br/>

<b>How to send JWT:</b><br/>
- Include in the request header:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Hospital updated successfully.<br/>
- 400 Bad Request → Validation failed or duplicate Name/Address.<br/>
- 401 Unauthorized → JWT missing/invalid/expired.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected error.<br/>
";

        #endregion


        #region DeleteHospital
        public const string DeleteHospitalSummary = "Delete a hospital from the system";
        public const string DeleteHospitalDescription =
            @"
<b>Purpose:</b><br/>
- Deletes an existing hospital.<br/>
- Ensures that no doctors or receptionists are associated with the hospital before deletion.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>This endpoint is protected.</b><br/>
- Must send a valid JWT token in the <b>Authorization</b> header.<br/><br/>

<b>Who can access:</b><br/>
- Only users with <b>Admin</b> role.<br/><br/>

<b>Request Parameters:</b><br/>
- <b>hospitalId</b> (GUID, required): The ID of the hospital to delete.<br/><br/>

<b>Business Logic:</b><br/>
1. Check if the <b>hospitalId</b> exists.<br/>
2. Verify that there are no doctors or receptionists associated with the hospital.<br/>
3. If both checks pass, delete the hospital from the database.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""Hospital Deleted Successfully""
}
</pre><br/>

<b>Validation / Business Errors (400 Bad Request):</b><br/>
- <b>Id Not Exist:</b> Hospital ID does not exist.<br/>
- <b>There Is Already Doctors Or Receptionists in this Hospital:</b> Cannot delete a hospital with associated employees.<br/><br/>

<b>Authentication Errors:</b><br/>
- <b>401 Unauthorized</b>: Missing, invalid, or expired JWT.<br/>
- <b>403 Forbidden</b>: JWT valid but user role is not Admin.<br/><br/>

<b>Other Errors (500 Internal Server Error):</b><br/>
- Unexpected database or server error.<br/><br/>

<b>How to send JWT:</b><br/>
- Include in the request header:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Hospital deleted successfully.<br/>
- 400 Bad Request → Validation or business logic error.<br/>
- 401 Unauthorized → JWT missing, invalid, or expired.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion
    }
}
