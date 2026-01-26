namespace TumorHospital.WebAPI.Documentation
{
    public static class ProfileDocs
    {
        #region GetPatientProfile

        public const string GetPatientProfileSummary = "Get patient profile details";
        public const string GetPatientProfileDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Doctor</b> role.<br/>
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
    <li>User is authenticated but does not have the Doctor role.</li>
  </ul><br/>

<b>Purpose:</b><br/>
- Retrieves the full profile information of a patient using the related Application User ID.<br/>
- Used to display patient personal and account details in profile pages or dashboards.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Route Parameters:</b><br/>
- <b>userId</b> (string, required):<br/>
  - The Application User ID associated with the patient.<br/><br/>

<b>Data Retrieved:</b><br/>
- Patient basic information.<br/>
- Linked user account details (from ApplicationUser).<br/>
- All data is returned as a <b>PatientProfileResponse</b> DTO.<br/><br/>

<b>Business Rules:</b><br/>
- The patient must exist and be linked to the provided user ID.<br/>
- If no patient is found, the request fails with an error response.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<br/>

<b>Error Responses:</b><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>Patient not found for the given user ID.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have permission to access this profile.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Use the returned data to populate patient profile screens.<br/>
- Handle <b>404</b> by showing a clear “Patient not found” message.<br/>
- Avoid calling this endpoint with an unauthenticated user.<br/>
";

        #endregion


        #region UpdatePatientProfile

        public const string UpdatePatientProfileSummary = "Update patient profile information";
        public const string UpdatePatientProfileDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Doctor</b> role.<br/>
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
    <li>User is authenticated but does not have the Doctor role.</li>
  </ul><br/>
- Rate limited using the <b>strict</b> policy (10 requests per minute).<br/><br/>

<b>Purpose:</b><br/>
- Updates the personal and account-related information of an existing patient.<br/>
- Modifies both the patient entity and the linked ApplicationUser data.<br/>
- Used in profile edit screens where patients update their personal details.<br/><br/>

<b>Request Type:</b><br/>
- HTTP PUT<br/><br/>

<b>Route Parameters:</b><br/>
- <b>userId</b> (string, required):<br/>
  - The Application User ID associated with the patient profile.<br/><br/>

<b>Request Body:</b><br/>
- <b>UpdatePatientProfileDto</b> object containing updated profile fields.<br/><br/>

<b>Validation Rules (FluentValidation):</b><br/>
- <b>FirstName</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 50 characters.</li>
  </ul>
- <b>LastName</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 50 characters.</li>
  </ul>
- <b>PhoneNumber</b>:
  <ul>
    <li>Required.</li>
    <li>Must be a valid phone number.</li>
    <li>Allowed length: 10 to 15 digits.</li>
    <li>Optional leading <b>+</b> is supported.</li>
  </ul>
- <b>Gender</b>:
  <ul>
    <li>Must be either <b>Male</b> or <b>Female</b>.</li>
  </ul>
- <b>Address</b>:
  <ul>
    <li>Required.</li>
  </ul>
- <b>DateOfBirth</b>:
  <ul>
    <li>Must be a date in the past.</li>
  </ul><br/>

<b>Business Rules:</b><br/>
- The patient must already exist and be linked to the provided user ID.<br/>
- Both patient data and user account data are updated in a single transaction.<br/>
- If the patient does not exist, the update operation fails and returns an error response.<br/><br/>

<b>Rate Limiting:</b><br/>
- Maximum of <b>10 requests per minute</b> per client.<br/>
- Additional requests within the time window are rejected immediately.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
Patient profile updated successfully
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Validation errors in the submitted profile data.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User is not allowed to update this profile.</li>
  </ul><br/>
- <b>429 Too Many Requests</b>:
  <ul>
    <li>Rate limit exceeded (more than 10 requests per minute).</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Validate fields client-side before submission to reduce API errors.<br/>
- Disable repeated rapid submissions to avoid rate-limit errors.<br/>
- Display validation messages returned from the API directly to the user.<br/>
- Show a clear success confirmation after profile update.<br/>
";

        #endregion


        #region GetDoctorProfile

        public const string GetDoctorProfileSummary = "Get doctor profile information";
        public const string GetDoctorProfileDescription =
            @"
<b>Authentication:</b><br/>
- JWT authentication required.<br/>
- Accessible only by users with the <b>Doctor</b> role.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves the complete profile information of a doctor.<br/>
- Returns both doctor-specific data and related user account details.<br/>
- Used to display doctor profile data in dashboards or profile screens.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Route Parameters:</b><br/>
- <b>userId</b> (string, required):<br/>
  - The Application User ID associated with the doctor account.<br/><br/>

<b>Business Rules:</b><br/>
- The doctor must exist and be linked to the provided user ID.<br/>
- The profile data is retrieved by joining Doctor data with ApplicationUser data.<br/>
- If no doctor is found, the request fails with a not found response.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  // DoctorProfileResponse object
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Doctor role.</li>
  </ul><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>Doctor profile not found for the provided user ID.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Use the authenticated doctor’s userId when calling this endpoint.<br/>
- Display profile data in read-only mode unless an update endpoint is provided.<br/>
- Handle the not-found case by redirecting the user or showing an error message.<br/>
";

        #endregion


        #region UpdateDoctorProfile

        public const string UpdateDoctorProfileSummary = "Update doctor profile information";
        public const string UpdateDoctorProfileDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Doctor</b> role.<br/>
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
    <li>User is authenticated but does not have the Doctor role.</li>
  </ul><br/>
- Rate limited using the <b>strict</b> policy (10 requests per minute).<br/><br/>

<b>Purpose:</b><br/>
- Updates the personal and professional profile information of an existing doctor.<br/>
- Modifies both the Doctor entity and the linked ApplicationUser data.<br/>
- Used in doctor profile edit screens.<br/><br/>

<b>Request Type:</b><br/>
- HTTP PUT<br/><br/>

<b>Route Parameters:</b><br/>
- <b>userId</b> (string, required):<br/>
  - The Application User ID associated with the doctor profile.<br/><br/>

<b>Request Body:</b><br/>
- <b>UpdateDoctorProfileDto</b> object containing updated doctor profile fields.<br/><br/>

<b>Validation Rules (FluentValidation):</b><br/>
- <b>FirstName</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 50 characters.</li>
  </ul>
- <b>LastName</b>:
  <ul>
    <li>Required.</li>
    <li>Maximum length: 50 characters.</li>
  </ul>
- <b>PhoneNumber</b>:
  <ul>
    <li>Required.</li>
    <li>Must be a valid phone number.</li>
    <li>Allowed length: 10 to 15 digits.</li>
    <li>Optional leading <b>+</b> is supported.</li>
  </ul>
- <b>Gender</b>:
  <ul>
    <li>Must be either <b>Male</b> or <b>Female</b>.</li>
  </ul>
- <b>Bio</b>:
  <ul>
    <li>Optional.</li>
    <li>Maximum length: 500 characters.</li>
  </ul><br/>

<b>Business Rules:</b><br/>
- The doctor must already exist and be linked to the provided user ID.<br/>
- Both doctor data and user account data are updated in a single transaction.<br/>
- If the doctor does not exist, the update operation fails and returns an error response.<br/><br/>

<b>Rate Limiting:</b><br/>
- Maximum of <b>10 requests per minute</b> per client.<br/>
- Additional requests within the time window are rejected immediately.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
Doctor profile updated successfully
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Validation errors in the submitted profile data.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User is not allowed to update this profile.</li>
  </ul><br/>
- <b>429 Too Many Requests</b>:
  <ul>
    <li>Rate limit exceeded (more than 10 requests per minute).</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Validate inputs on the client side before submission.<br/>
- Prevent rapid repeated submissions to avoid rate-limit errors.<br/>
- Display validation messages returned from the API clearly to the user.<br/>
- Show a success confirmation after profile update.<br/>
"
;

        #endregion


        #region GetReceptionistProfile

        public const string GetReceptionistProfileSummary = "Get receptionist profile information";
        public const string GetReceptionistProfileDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Receptionist</b> role.<br/>
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
    <li>User is authenticated but does not have the Receptionist role.</li>
  </ul><br/>

<b>Purpose:</b><br/>
- Retrieves the complete profile information of a receptionist.<br/>
- Returns both receptionist-specific data and linked ApplicationUser information.<br/>
- Used to display receptionist profile details in the system.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Route Parameters:</b><br/>
- <b>userId</b> (string, required):<br/>
  - The Application User ID associated with the receptionist profile.<br/><br/>

<b>Business Rules:</b><br/>
- The receptionist must already exist and be linked to the provided user ID.<br/>
- If no receptionist is found for the given user ID, the request fails.<br/><br/>

<b>Success Response (200 OK):</b><br/>
- Returns a <b>ReceptionistProfileResponse</b> object containing receptionist details.<br/><br/>

<b>Error Responses:</b><br/>
- <b>404 Not Found</b>:
  <ul>
    <li>No receptionist profile exists for the provided user ID.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have permission to access this profile.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Use this endpoint to load receptionist profile data on profile or dashboard screens.<br/>
- Handle the <b>404</b> response gracefully if the profile does not exist.<br/>
- Do not allow access if the logged-in user is not a receptionist.<br/>
"
;

        #endregion


        #region UpdateReceptionistProfile

        public const string UpdateReceptionistProfileSummary = "Update receptionist profile information";
        public const string UpdateReceptionistProfileDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Receptionist</b> role.<br/>
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
    <li>User is authenticated but does not have the Receptionist role.</li>
  </ul><br/>
- Rate limited using the <b>strict</b> policy (10 requests per minute).<br/><br/>

<b>Purpose:</b><br/>
- Updates the personal and account-related information of an existing receptionist.<br/>
- Modifies both the receptionist entity and the linked ApplicationUser data.<br/>
- Used in profile edit screens where receptionists update their personal details.<br/><br/>

<b>Request Type:</b><br/>
- HTTP PUT<br/><br/>

<b>Route Parameters:</b><br/>
- <b>userId</b> (string, required):<br/>
  - The Application User ID associated with the receptionist profile.<br/><br/>

<b>Request Body:</b><br/>
- <b>UpdateReceptionistProfileDto</b> object containing updated profile fields.<br/><br/>

<b>Validation Rules:</b><br/>
- All fields are validated using <b>FluentValidation</b> before updating.<br/>
- If validation fails, the update operation is not executed.<br/><br/>

<b>Business Rules:</b><br/>
- The receptionist must already exist and be linked to the provided user ID.<br/>
- Both receptionist data and user account data are updated in a single transaction.<br/>
- If the receptionist does not exist, the update operation fails silently and returns an error response.<br/><br/>

<b>Rate Limiting:</b><br/>
- Maximum of <b>10 requests per minute</b> per client.<br/>
- Additional requests within the time window are rejected immediately.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
Receptionist profile updated successfully
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Validation errors in the submitted profile data.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User is not allowed to update this profile.</li>
  </ul><br/>
- <b>429 Too Many Requests</b>:
  <ul>
    <li>Rate limit exceeded (more than 10 requests per minute).</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Disable repeated rapid submissions to avoid rate-limit errors.<br/>
- Display validation messages returned from the API directly to the user.<br/>
- Show a clear success confirmation after profile update.<br/>
";

        #endregion
    }
}
