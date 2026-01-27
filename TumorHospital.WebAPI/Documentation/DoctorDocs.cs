namespace TumorHospital.WebAPI.Documentation
{
    public static class DoctorDocs
    {
        #region UploadProfilePicture


        public const string UploadProfilePictureSummary = "Upload or update doctor profile picture";
        public const string UploadProfilePictureDescription =
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
- Uploads or updates the profile picture of the authenticated doctor.<br/>
- If a profile picture already exists, it will be replaced.<br/><br/>

<b>Request Type:</b><br/>
- multipart/form-data<br/><br/>

<b>Request Parameters:</b><br/>
- <b>file</b> (IFormFile, required): Image file to upload.<br/>
- <b>userId</b> (string, required): Doctor user ID.<br/><br/>

<b>File Validation Rules:</b><br/>
- File must not be null or empty.<br/>
- Maximum allowed file size is <b>1 MB</b>.<br/>
- Allowed file extensions only:
  <ul>
    <li>.png</li>
    <li>.jpg</li>
    <li>.jpeg</li>
  </ul><br/>

<b>Business Rules:</b><br/>
- Doctor must exist in the system.<br/>
- The authenticated doctor is allowed to upload or update only his own profile picture.<br/>
- Existing profile picture will be replaced if present.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Profile Picture Uploaded Successfully""
}
</pre><br/>

<b>Validation & Error Scenarios (400 Bad Request):</b><br/>
- No file uploaded.<br/>
- File size exceeds 1 MB.<br/>
- Invalid file extension.<br/>
- Doctor does not exist.<br/><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Profile picture uploaded successfully.<br/>
- 400 Bad Request → Validation or business rule failure.<br/>
- 401 Unauthorized → Missing, invalid, or expired JWT token.<br/>
- 403 Forbidden → User does not have Doctor role.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Always send JWT token in the Authorization header.<br/>
- Use multipart/form-data when uploading the image.<br/>
- It is recommended to validate image size and extension on the client side before sending the request.<br/>
";

        #endregion


        #region GetDoctors


        public const string GetDoctorsSummary = "Get paginated list of doctors with optional filters (Main Page)";
        public const string GetDoctorsDescription =
            @"
<b>Authentication:</b><br/>
- <b>No authentication required.</b><br/>
- This endpoint is <b>public</b> and can be accessed without a JWT token.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves a paginated list of active doctors.<br/>
- Supports optional filtering by:
  <ul>
    <li>Working day</li>
    <li>Surgeon status</li>
    <li>Hospital government</li>
  </ul><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Query Parameters:</b><br/>
- <b>pageNumber</b> (int, required):<br/>
  - Page number for pagination (starts from 1).<br/><br/>

- <b>workDay</b> (string, optional):<br/>
  - Filters doctors by working day.<br/>
  - Allowed values (case-insensitive):
    <ul>
      <li>monday</li>
      <li>tuesday</li>
      <li>wednesday</li>
      <li>thursday</li>
      <li>friday</li>
      <li>saturday</li>
      <li>sunday</li>
    </ul><br/>

- <b>isSurgeon</b> (bool, optional):<br/>
  - true → returns only surgeons.<br/>
  - false → returns only non-surgeons.<br/>
  - null → returns both.<br/><br/>

- <b>government</b> (string, optional):<br/>
  - Filters doctors by hospital government name.<br/>
  - Example: Cairo, Giza, Alexandria.<br/><br/>

- <b>specializationName</b> (string, optional):<br/>
  - Filters doctors by specialization name.<br/>

<b>Filtering Rules:</b><br/>
- Only <b>active doctors</b> are returned.<br/>
- If <b>workDay</b> is provided, only doctors who work on that day are returned.<br/>
- Filters can be combined together.<br/><br/>

<b>Pagination:</b><br/>
- Page size is fixed to <b>15 doctors per page</b>.<br/>
- Results are wrapped inside a paginated response object.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""items"": [
    {
      ""id"": ""string"",
      ""fullName"": ""Doctor Full Name"",
      ""profileImageUrl"": ""string | null"",
      ""gender"": ""Male | Female""
    }
  ],
  ""pageNumber"": 1,
  ""pageSize"": 15,
  ""totalCount"": 120
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Invalid page number.</li>
    <li>Invalid workDay value.</li>
  </ul><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Doctors retrieved successfully.<br/>
- 400 Bad Request → Invalid query parameters.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Always send <b>pageNumber</b>.<br/>
- All other filters are optional.<br/>
- Handle empty results gracefully (no doctors found).<br/>
";

        #endregion


        #region GetDoctor


        public const string GetDoctorSummary = "Get detailed information about a specific doctor";
        public const string GetDoctorDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Only users with the <b>Patient</b> role can access this endpoint.<br/>
- A valid access token must be sent in the <b>Authorization</b> header.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves full details of a specific doctor.<br/>
- Includes personal information, specialization, costs, working days, and availability.<br/>
- Determines whether the patient can book:
  <ul>
    <li>A consultation</li>
    <li>A follow-up appointment</li>
  </ul><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Route Parameters:</b><br/>
- <b>doctorId</b> (string, required):<br/>
  - The unique identifier of the doctor.<br/><br/>

<b>Query Parameters:</b><br/>
- <b>patientId</b> (string, required):<br/>
  - The unique identifier of the authenticated patient.<br/><br/>

<b>Doctor Validation Rules:</b><br/>
- The doctor must exist.<br/>
- The doctor must be <b>active</b> (User.IsActive = true).<br/>
- If the doctor does not exist or is inactive, an error is returned.<br/><br/>

<b>Working Days & Availability:</b><br/>
- Only the doctor's scheduled working days are returned.<br/>
- Each working day includes:
  <ul>
    <li>Day name</li>
    <li>Start time</li>
    <li>End time</li>
    <li>Date of the day in the current week</li>
    <li>Availability status</li>
  </ul><br/>
- A day is marked as <b>available</b> only if:
  <ul>
    <li>The day is not in the past</li>
    <li>The doctor still has available appointments on that day</li>
  </ul><br/>

<b>Appointment Rules:</b><br/>
- <b>Consultation appointment:</b><br/>
  - The patient can book a consultation only if there is <b>no existing pending or approved</b>
    appointment with the same doctor.<br/><br/>

- <b>Follow-up appointment:</b><br/>
  - The patient can book a follow-up only if there is at least one
    <b>completed consultation</b> appointment with the same doctor.<br/><br/>

<b>Cost Rules:</b><br/>
- <b>SurgeryCost</b> is returned only if the doctor is a surgeon.<br/>
- If the doctor is not a surgeon, <b>SurgeryCost</b> will be <b>null</b>.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""id"": ""string"",
  ""fullName"": ""Doctor Full Name"",
  ""profileImageUrl"": ""string | null"",
  ""gender"": ""Male | Female"",
  ""bio"": ""string"",
  ""specialization"": ""string"",
  ""isSurgeon"": true,
  ""consultationCost"": 200,
  ""followUpCost"": 100,
  ""surgeryCost"": 5000,
  ""location"": ""Hospital Address - Government"",
  ""workingDays"": [
    {
      ""day"": ""Monday"",
      ""fromTime"": ""09:00"",
      ""toTime"": ""14:00"",
      ""date"": ""2026-01-27"",
      ""isAvailable"": true
    }
  ],
  ""isAbleToAppointConsultation"": true,
  ""isAbleToAppointFollowUp"": false
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Doctor not found.</li>
    <li>Doctor is inactive.</li>
  </ul><br/>

- <b>401 Unauthorized</b>:
  <ul>
    <li>Missing or invalid JWT token.</li>
  </ul><br/>

- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Patient role.</li>
  </ul><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Doctor details retrieved successfully.<br/>
- 400 Bad Request → Invalid doctor or request data.<br/>
- 401 Unauthorized → Authentication required.<br/>
- 403 Forbidden → Access denied.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Always send <b>patientId</b> with the request.<br/>
- Use <b>isAbleToAppointConsultation</b> and <b>isAbleToAppointFollowUp</b>
  to control booking UI logic.<br/>
- Handle unavailable days gracefully.<br/>
";

        #endregion


        #region GetDoctorAppointments


        public const string GetDoctorAppointmentsSummary = "Get paginated list of doctor's appointments with optional filters";
        public const string GetDoctorAppointmentsDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Only users with the <b>Doctor</b> role can access this endpoint.<br/>
- A valid access token must be sent in the <b>Authorization</b> header.<br/><br/>

<b>Purpose:</b><br/>
- Retrieves a paginated list of appointments for a specific doctor.<br/>
- Supports optional filtering by:
  <ul>
    <li>Appointment reason</li>
    <li>Appointment status</li>
  </ul><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Query Parameters:</b><br/>
- <b>pageNumber</b> (int, required):<br/>
  - Page number for pagination (starts from 1).<br/><br/>

- <b>doctorId</b> (string, required):<br/>
  - The unique identifier of the authenticated doctor.<br/><br/>

- <b>appointmentReason</b> (string, optional):<br/>
  - Filters appointments by reason.<br/>
  - Allowed values (case-insensitive):
    <ul>
      <li>Consultation</li>
      <li>FollowUp</li>
      <li>Surgery</li>
    </ul><br/>

- <b>appointmentStatus</b> (string, optional):<br/>
  - Filters appointments by status.<br/>
  - Allowed values (case-insensitive):
    <ul>
      <li>Pending</li>
      <li>Approved</li>
      <li>Completed</li>
      <li>Cancelled</li>
    </ul><br/>

<b>Filtering Rules:</b><br/>
- Only appointments that belong to the provided <b>doctorId</b> are returned.<br/>
- Filters can be used individually or combined together.<br/>
- If an invalid enum value is provided, a <b>400 Bad Request</b> is returned.<br/><br/>

<b>Pagination:</b><br/>
- Page size is fixed to <b>15 appointments per page</b>.<br/>
- Results are wrapped inside a paginated response object.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""items"": [
    {
      ""appointmentId"": ""string"",
      ""patientName"": ""Patient Full Name"",
      ""doctorName"": ""Doctor Full Name"",
      ""doctorImagePath"": ""string"",
      ""reason"": ""Consultation"",
      ""dayOfWeek"": ""Monday"",
      ""fromTime"": ""10:00"",
      ""toTime"": ""10:30"",
      ""status"": ""Pending"",
      ""requestCreatedAt"": ""2026-01-20T10:00:00"",
      ""attendenceDate"": ""2026-01-25""
    }
  ],
  ""pageNumber"": 1,
  ""pageSize"": 15,
  ""totalCount"": 40
}
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Invalid appointment reason.</li>
    <li>Invalid appointment status.</li>
  </ul><br/>

- <b>401 Unauthorized</b>:
  <ul>
    <li>Missing or invalid JWT token.</li>
  </ul><br/>

- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Doctor role.</li>
  </ul><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Appointments retrieved successfully.<br/>
- 400 Bad Request → Invalid query parameters.<br/>
- 401 Unauthorized → Authentication required.<br/>
- 403 Forbidden → Access denied.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Always send <b>pageNumber</b> and <b>doctorId</b>.<br/>
- Filters are optional and can be combined.<br/>
- Handle empty results gracefully (no appointments found).<br/>
";

        #endregion
    }
}
