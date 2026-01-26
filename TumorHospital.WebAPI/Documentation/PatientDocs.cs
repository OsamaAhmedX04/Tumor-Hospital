namespace TumorHospital.WebAPI.Documentation
{
    public static class PatientDocs
    {
        #region GetAppointments
        public const string GetAppointmentsSummary = "Get patient appointments with optional filtering and pagination";
        public const string GetAppointmentsDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Patient</b> role.<br/>
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
    <li>User is authenticated but does not have the Patient role.</li>
  </ul><br/>

<b>Purpose:</b><br/>
- Retrieves a paginated list of appointments for a specific patient.<br/>
- Allows optional filtering by appointment reason and appointment status.<br/>
- Designed for patient dashboards and appointment history screens.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Query Parameters:</b><br/>
- <b>pageNumber</b> (int, required):<br/>
  - The page number for pagination (starts from 1).<br/><br/>

- <b>patientId</b> (string, required):<br/>
  - The unique identifier of the patient whose appointments are requested.<br/><br/>

- <b>appointmentReason</b> (string, optional):<br/>
  - Filters appointments by reason.<br/>
  - Must match a valid <b>AppointmentReason</b> enum value.<br/><br/>

- <b>appointmentStatus</b> (string, optional):<br/>
  - Filters appointments by status.<br/>
  - Must match a valid <b>AppointmentStatus</b> enum value.<br/><br/>

<b>Filtering Logic:</b><br/>
- If only <b>appointmentReason</b> is provided, results are filtered by reason.<br/>
- If only <b>appointmentStatus</b> is provided, results are filtered by status.<br/>
- If both are provided, results are filtered by both reason and status.<br/>
- If neither is provided, all patient appointments are returned.<br/><br/>

<b>Validation Rules:</b><br/>
- Invalid enum values for <b>appointmentReason</b> or <b>appointmentStatus</b> result in an error response.<br/><br/>

<b>Pagination:</b><br/>
- Page size is fixed at <b>15 appointments per page</b>.<br/>
- Results are wrapped in a paginated response object including metadata.<br/><br/>

<b>Success Response (200 OK):</b><br/>
- Returns a paginated list of appointments with the following data:<br/>
  <ul>
    <li>Appointment ID</li>
    <li>Patient name</li>
    <li>Doctor name and profile image</li>
    <li>Appointment reason</li>
    <li>Day of week</li>
    <li>Time range (from / to)</li>
    <li>Appointment status</li>
    <li>Prescription existence flag</li>
    <li>Request creation date</li>
    <li>Attendance date</li>
  </ul><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Invalid appointment reason value.</li>
    <li>Invalid appointment status value.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have permission to access this endpoint.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server-side error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Always send <b>pageNumber</b> when requesting appointments.<br/>
- Validate enum values on the frontend to avoid unnecessary API errors.<br/>
- Use returned pagination metadata to implement infinite scroll or page navigation.<br/>
";
        #endregion


        #region GetBills
        public const string GetBillsSummary = "Get patient bills with pagination";
        public const string GetBillsDescription =
            @"
<b>Authentication & Authorization:</b><br/>
- JWT token is REQUIRED.<br/>
- The user must be authenticated and have the <b>Patient</b> role.<br/>
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
    <li>User is authenticated but does not have the Patient role.</li>
  </ul><br/>

<b>Purpose:</b><br/>
- Retrieves a paginated list of all bills associated with a specific patient.<br/>
- Used in patient billing history and financial overview screens.<br/>
- Allows patients to review payment status and appointment-related charges.<br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Query Parameters:</b><br/>
- <b>pageNumber</b> (int, required):<br/>
  - The page number for pagination (starts from 1).<br/><br/>

- <b>patientId</b> (string, required):<br/>
  - The unique identifier of the patient whose bills are requested.<br/><br/>

<b>Validation Rules:</b><br/>
- The provided <b>patientId</b> must reference an existing patient record.<br/>
- If the patient does not exist, the request fails with an error response.<br/><br/>

<b>Pagination:</b><br/>
- Page size is fixed at <b>20 bills per page</b>.<br/>
- Results are returned in a paginated response object including metadata.<br/><br/>

<b>Returned Bill Data:</b><br/>
Each bill item includes:
<ul>
  <li>Bill ID</li>
  <li>Bill code</li>
  <li>Creation date</li>
  <li>Patient full name</li>
  <li>Related appointment attendance date</li>
  <li>Bill status</li>
  <li>Total amount</li>
</ul><br/>

<b>Success Response (200 OK):</b><br/>
- Returns a paginated list of patient bills with full billing details.<br/><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Patient does not exist.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User is not allowed to access this resource.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server-side error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Always include <b>pageNumber</b> when requesting bills.<br/>
- Use pagination metadata to implement page navigation or infinite scrolling.<br/>
- Display bill status and total amount clearly for better user understanding.<br/>
";
        #endregion
    }
}
