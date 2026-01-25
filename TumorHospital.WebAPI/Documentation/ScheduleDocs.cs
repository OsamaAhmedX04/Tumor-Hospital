namespace TumorHospital.WebAPI.Documentation
{
    public static class ScheduleDocs
    {
        #region GetDoctorSchedule

        public const string GetDoctorScheduleSummary = "Get doctor weekly schedule with approved appointments";
        public const string GetDoctorScheduleDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Accessible by users with:
  <ul>
    <li><b>Admin</b> role</li>
    <li><b>Doctor</b> role</li>
  </ul><br/><br/>

<b>Purpose:</b><br/>
- Retrieves the full weekly work schedule of a specific doctor.<br/>
- Includes:
  <ul>
    <li>Working days</li>
    <li>Start and end times per day</li>
    <li>All <b>approved appointments</b> for each day</li>
  </ul><br/><br/>

<b>Request Type:</b><br/>
- HTTP GET<br/><br/>

<b>Query Parameters:</b><br/>
- <b>doctorId</b> (string, required):<br/>
  - The unique identifier of the doctor (ApplicationUserId).<br/><br/>

<b>Business Rules:</b><br/>
- Only appointments with status <b>Approved</b> are returned.<br/>
- Appointments are grouped under their corresponding working day.<br/>
- Pending or cancelled appointments are excluded.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
[
  {
    ""dayOfWeek"": ""Monday"",
    ""startTime"": ""09:00"",
    ""endTime"": ""17:00"",
    ""appointmentDurations"": [
      {
        ""appointmentReason"": ""Consultation"",
        ""startTime"": ""10:00"",
        ""endTime"": ""10:30"",
        ""patientName"": ""Patient Full Name""
      }
    ]
  }
]
</pre><br/>

<b>Error Responses:</b><br/>
- <b>400 Bad Request</b>:
  <ul>
    <li>Invalid or missing doctorId.</li>
  </ul><br/>
- <b>401 Unauthorized</b>:
  <ul>
    <li>JWT token is missing or invalid.</li>
  </ul><br/>
- <b>403 Forbidden</b>:
  <ul>
    <li>User does not have Admin or Doctor role.</li>
  </ul><br/>
- <b>500 Internal Server Error</b>:
  <ul>
    <li>Unexpected server error.</li>
  </ul><br/>

<b>Frontend Notes:</b><br/>
- Use this endpoint to render doctor calendar views.<br/>
- Time slots inside <b>appointmentDurations</b> represent already reserved times.<br/>
- Empty appointment list means the doctor is available during that working period.<br/>
";

        #endregion


        #region AddSchedule

        public const string AddScheduleSummary = "Add a new working schedule for a doctor";
        public const string AddScheduleDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Accessible only by users with the <b>Admin</b> role.<br/><br/>

<b>Purpose:</b><br/>
- Creates a new weekly working schedule for a specific doctor.<br/>
- Each doctor can work on a limited number of days per week.<br/>
- Each working day represents an <b>8-hour shift</b> starting from the provided start time.<br/><br/>

<b>Request Type:</b><br/>
- HTTP POST<br/><br/>

<b>Query Parameters:</b><br/>
- <b>doctorId</b> (string, required):<br/>
  - The unique identifier of the doctor (ApplicationUserId).<br/><br/>

<b>Request Body:</b><br/>
- <b>DoctorScheduleDto</b> object containing:
  <ul>
    <li><b>dayOfWeek</b> (string, required)</li>
    <li><b>startTime</b> (TimeSpan, required)</li>
  </ul><br/>

<b>Validation Rules:</b><br/>

<b>dayOfWeek:</b><br/>
<ul>
  <li>Required.</li>
  <li>Friday is <b>not allowed</b> (official holiday).</li>
  <li>Allowed values only:
    <ul>
      <li>Saturday</li>
      <li>Sunday</li>
      <li>Monday</li>
      <li>Tuesday</li>
      <li>Wednesday</li>
      <li>Thursday</li>
    </ul>
  </li>
  <li>Doctor cannot have duplicate schedules for the same day.</li>
</ul><br/>

<b>startTime:</b><br/>
<ul>
  <li>Required.</li>
  <li>Must be greater than or equal to <b>06:00 AM</b>.</li>
  <li>Must be less than or equal to <b>04:00 PM</b>.</li>
  <li>End time is calculated automatically as <b>startTime + 8 hours</b>.</li>
</ul><br/>

<b>Business Rules:</b><br/>
<ul>
  <li>A doctor can work a maximum of <b>5 days per week</b>.</li>
  <li>Adding a schedule for an already assigned day is not allowed.</li>
  <li>The created schedule is marked as <b>Available</b> by default.</li>
</ul><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Schedule Created Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
<ul>
  <li><b>400 Bad Request</b>:
    <ul>
      <li>Invalid dayOfWeek value.</li>
      <li>Friday is not allowed.</li>
      <li>Invalid startTime range.</li>
      <li>Duplicate working day for the doctor.</li>
      <li>Doctor already has 5 working days.</li>
    </ul>
  </li>
  <li><b>401 Unauthorized</b>: JWT token is missing or invalid.</li>
  <li><b>403 Forbidden</b>: User does not have Admin role.</li>
  <li><b>500 Internal Server Error</b>: Unexpected server error.</li>
</ul><br/>

<b>Frontend Notes:</b><br/>
<ul>
  <li>Prevent sending Friday as a working day.</li>
  <li>Limit start time selection between 06:00 and 16:00.</li>
  <li>Disable adding more than 5 schedules per doctor.</li>
  <li>Show validation messages returned from the API directly to the user.</li>
</ul>
"
;

        #endregion


        #region UpdateSchedule

        public const string UpdateScheduleSummary = "Update an existing doctor working schedule";
        public const string UpdateScheduleDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Accessible only by users with the <b>Admin</b> role.<br/><br/>

<b>Purpose:</b><br/>
- Updates an existing working schedule for a specific doctor.<br/>
- Allows changing the working day and start time under strict business rules.<br/>
- Schedule duration remains <b>8 hours</b> starting from the provided start time.<br/><br/>

<b>Request Type:</b><br/>
- HTTP PUT<br/><br/>

<b>Query Parameters:</b><br/>
<ul>
  <li><b>scheduleId</b> (Guid, required): The unique identifier of the schedule.</li>
  <li><b>doctorId</b> (string, required): The unique identifier of the doctor.</li>
</ul><br/>

<b>Request Body:</b><br/>
- <b>DoctorScheduleDto</b> object containing:
  <ul>
    <li><b>dayOfWeek</b> (string, required)</li>
    <li><b>startTime</b> (TimeSpan, required)</li>
  </ul><br/>

<b>Validation Rules:</b><br/>
- <b>dayOfWeek</b>:
  <ul>
    <li>Required.</li>
    <li>Allowed values:
      <ul>
        <li>Saturday</li>
        <li>Sunday</li>
        <li>Monday</li>
        <li>Tuesday</li>
        <li>Wednesday</li>
        <li>Thursday</li>
      </ul>
    </li>
    <li><b>Friday is not allowed</b> (official holiday).</li>
    <li>Doctor cannot work on duplicate days.</li>
  </ul><br/>

- <b>startTime</b>:
  <ul>
    <li>Required.</li>
    <li>Must be between <b>06:00 AM</b> and <b>04:00 PM</b>.</li>
    <li>End time is calculated automatically as <b>startTime + 8 hours</b>.</li>
  </ul><br/>

<b>Business Rules:</b><br/>
<ul>
  <li>Schedule cannot be updated if there are <b>Approved</b> or <b>Pending</b> appointments on that day.</li>
  <li>Schedule can only be modified if no active appointments exist for the selected day.</li>
  <li>Duplicate working days for the same doctor are not allowed.</li>
  <li>LastModified date is updated automatically.</li>
</ul><br/>

<b>Business Logic Flow:</b><br/>
<ol>
  <li>Validate request body using FluentValidation.</li>
  <li>Check if the schedule exists.</li>
  <li>Check for existing appointments on the schedule day.</li>
  <li>Ensure no duplicate working day exists for the doctor.</li>
  <li>Update schedule using ExecuteUpdateAsync.</li>
</ol><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Schedule Updated Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
<ul>
  <li><b>400 Bad Request</b>:
    <ul>
      <li>Validation errors.</li>
      <li>Duplicate working day.</li>
      <li>Existing appointments prevent updating the schedule.</li>
    </ul>
  </li>
  <li><b>401 Unauthorized</b>: Missing or invalid JWT.</li>
  <li><b>403 Forbidden</b>: User is not Admin.</li>
  <li><b>500 Internal Server Error</b>: Unexpected server error.</li>
</ul><br/>

<b>Frontend Notes:</b><br/>
<ul>
  <li>Prevent selecting duplicate working days.</li>
  <li>Show server validation messages directly to the user.</li>
</ul>
";

        #endregion


        #region DeleteSchedule

        public const string DeleteScheduleSummary = "Delete an existing doctor working schedule";
        public const string DeleteScheduleDescription =
            @"
<b>Authentication:</b><br/>
- <b>JWT authentication required.</b><br/>
- Accessible only by users with the <b>Admin</b> role.<br/><br/>

<b>Purpose:</b><br/>
- Deletes an existing working schedule for a specific doctor.<br/>
- Deletion is restricted by minimum working days and existing appointments.<br/><br/>

<b>Request Type:</b><br/>
- HTTP DELETE<br/><br/>

<b>Query Parameters:</b><br/>
<ul>
  <li><b>scheduleId</b> (Guid, required): The unique identifier of the schedule.</li>
  <li><b>doctorId</b> (string, required): The unique identifier of the doctor.</li>
</ul><br/>

<b>Business Rules:</b><br/>
<ul>
  <li>Each doctor must have at least <b>3 working days</b> per week.</li>
  <li>Schedule cannot be deleted if the doctor has:
    <ul>
      <li><b>Approved</b> appointments, or</li>
      <li><b>Pending</b> appointments</li>
    </ul>
    on the same day.
  </li>
  <li>Schedule can only be deleted if there are no active appointments on that day.</li>
</ul><br/>

<b>Business Logic Flow:</b><br/>
<ol>
  <li>Count total working days for the doctor.</li>
  <li>Prevent deletion if remaining days would be less than 3.</li>
  <li>Check for Approved or Pending appointments on the schedule day.</li>
  <li>Delete the schedule if all conditions are satisfied.</li>
</ol><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Schedule Deleted Successfully""
}
</pre><br/>

<b>Error Responses:</b><br/>
<ul>
  <li><b>400 Bad Request</b>:
    <ul>
      <li>Doctor would have less than 3 working days.</li>
      <li>Existing Approved or Pending appointments prevent deletion.</li>
    </ul>
  </li>
  <li><b>401 Unauthorized</b>: Missing or invalid JWT.</li>
  <li><b>403 Forbidden</b>: User does not have Admin role.</li>
  <li><b>500 Internal Server Error</b>: Unexpected server error.</li>
</ul><br/>

<b>Frontend Notes:</b><br/>
<ul>
  <li>Disable delete option if doctor has exactly 3 working days.</li>
  <li>Warn user if appointments exist on the selected day.</li>
  <li>Show API validation messages directly to the user.</li>
</ul>
";

        #endregion
    }
}
