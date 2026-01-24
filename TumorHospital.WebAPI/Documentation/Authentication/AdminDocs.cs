namespace TumorHospital.WebAPI.Documentation.Authentication
{
    public static class AdminDocs
    {

        #region CreateNewDoctor

        public const string CreateNewDoctorSummary = "Create a new doctor account";
        public const string CreateNewDoctorDescription =
            @"
<b>Access Control:</b><br/>
- Requires <b>Admin</b> role.<br/>
- JWT token must be provided with Admin privileges.<br/><br/>

<b>Purpose:</b><br/>
- Creates a new doctor account and links it to a hospital and specialization.<br/>
- Generates a random password and sends it to the doctor via email.<br/><br/>

<b>Request Body Validation Rules:</b><br/>

<b>Personal Information:</b><br/>
- <b>FirstName</b>: Required, max length 30 characters.<br/>
- <b>LastName</b>: Required, max length 30 characters.<br/>
- <b>Email</b>: Required, must be a valid email format, must be unique.<br/>
- <b>Gender</b>: Required, allowed values: <b>Male</b> or <b>Female</b> only.<br/><br/>

<b>Professional Information:</b><br/>
- <b>SpecializationName</b>: Required, must exist in the system.<br/>
- <b>HospitalName</b>: Required, must exist in the system.<br/>
- Hospital must not exceed its maximum allowed number of doctors.<br/><br/>

<b>Financial Rules:</b><br/>
- <b>ConsultationCost</b>: Must be greater than 0.<br/>
- <b>FollowUpCost</b>: Must be greater than 0 and less than or equal to ConsultationCost.<br/>
- <b>IsSurgeon</b>: Required (true or false).<br/>
- <b>SurgeryCost</b>:<br/>
&nbsp;&nbsp;• Required only if IsSurgeon = true.<br/>
&nbsp;&nbsp;• Must be greater than ConsultationCost.<br/>
&nbsp;&nbsp;• Must be greater than FollowUpCost.<br/><br/>

<b>Schedules Rules:</b><br/>
- <b>Schedules</b>: Required.<br/>
- Must contain between <b>3 and 5</b> working days per week.<br/>
- Duplicate days are not allowed.<br/>
- Each schedule item must pass its own validation rules (DoctorScheduleDtoValidator).<br/><br/>

<b>Business Logic:</b><br/>
1. Validate request body using FluentValidation.<br/>
2. Ensure specialization exists.<br/>
3. Ensure hospital exists and has available doctor slots.<br/>
4. Create Identity user with a generated random password.<br/>
5. Assign the user to <b>InActiveDoctor</b> role.<br/>
6. Create doctor record and link it to hospital and specialization.<br/>
7. Send login credentials to doctor's email.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""New Doctor Account Created Successfully""
}
</pre><br/>

<b>Validation Error Response (400 Bad Request):</b><br/>
- Returned when request validation fails or business rules are violated.<br/>
- Response contains a list of field-specific errors.<br/><br/>

<b>Possible Errors:</b><br/>
- Specialization does not exist.<br/>
- Hospital does not exist.<br/>
- Hospital has reached maximum number of doctors.<br/>
- Identity creation failure (email already exists, password rules, etc).<br/>
";

        #endregion


        #region CreateNewReceptionist

        public const string CreateNewReceptionistSummary = "Create a new receptionist account";
        public const string CreateNewReceptionistDescription =
            @"
<b>Access Control:</b><br/>
- Requires <b>Admin</b> role.<br/>
- <b>This endpoint is protected.</b><br/>
- You must send a valid <b>JWT token</b> in the request header.<br/><br/>

<b>How to send the JWT:</b><br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Purpose:</b><br/>
- Creates a new receptionist account in the system.<br/>
- Links the receptionist to a specific hospital.<br/>
- Generates a random password automatically.<br/>
- Sends login credentials to the receptionist via email.<br/><br/>

<b>Request Body Validation Rules:</b><br/>

<b>Personal Information:</b><br/>
- <b>FirstName</b>: Required, max length 30 characters.<br/>
- <b>LastName</b>: Required, max length 30 characters.<br/>
- <b>Email</b>: Required, must be a valid email format, must be unique in the system.<br/>
- <b>Gender</b>: Required, allowed values: <b>Male</b> or <b>Female</b> only.<br/>
- <b>Address</b>: Required, max length 100 characters.<br/><br/>

<b>Hospital Rules:</b><br/>
- <b>HospitalName</b>: Must exist in the system.<br/>
- Hospital must not exceed its maximum allowed number of receptionists.<br/><br/>

<b>Business Logic Flow:</b><br/>
1. Validate request body using FluentValidation.<br/>
2. Check if the hospital exists.<br/>
3. Check hospital receptionist capacity (MaxNumberOfReceptionists).<br/>
4. Create Identity user with a generated random password.<br/>
5. Assign role <b>InActiveReceptionistRole</b> to the user.<br/>
6. Create receptionist record and link it to the hospital.<br/>
7. Save changes to database.<br/>
8. Send email with login credentials to the receptionist.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""New Receptionist Account Created Successfully""
}
</pre><br/>

<b>Authentication Errors:</b><br/>
- <b>401 Unauthorized</b>:<br/>
  - JWT token is missing.<br/>
  - JWT token is invalid or expired.<br/><br/>

- <b>403 Forbidden</b>:<br/>
  - JWT token is valid, but user role is not <b>Admin</b>.<br/><br/>

<b>Validation Errors (400 Bad Request):</b><br/>
- Missing required fields.<br/>
- Invalid email format.<br/>
- Invalid gender value.<br/>
- Field length violations.<br/>
- Business rule violations.<br/><br/>

<b>Business Errors:</b><br/>
- Hospital does not exist.<br/>
- Hospital has reached the maximum number of receptionists.<br/>
- Email already exists in the system.<br/>
- Identity user creation failure.<br/><br/>

<b>Side Effects:</b><br/>
- New user is created in Identity system.<br/>
- Receptionist entity is created in database.<br/>
- Role assigned: <b>InActiveReceptionistRole</b>.<br/>
- Email with credentials is sent to the receptionist.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Receptionist created successfully.<br/>
- 400 Bad Request → Validation or business rule failure.<br/>
- 401 Unauthorized → Missing / invalid / expired JWT.<br/>
- 403 Forbidden → Not Admin role.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion


        #region DeleteDoctor

        public const string DeleteDoctorSummary = "Delete a doctor account";
        public const string DeleteDoctorDescription =
            @"
<b>Access Control:</b><br/>
- Requires <b>Admin</b> role.<br/>
- Endpoint is protected using <b>JWT Authentication</b>.<br/><br/>

<b>How to send the JWT:</b><br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Purpose:</b><br/>
- Permanently deletes a doctor account from the system.<br/>
- Removes the doctor from the Identity system.<br/>
- Deletes the doctor's profile image from storage if it exists.<br/><br/>

<b>Route Parameters:</b><br/>
- <b>doctorId</b>:<br/>
  - Required.<br/>
  - Identity User Id of the doctor to be deleted.<br/><br/>

<b>Business Rules & Logic:</b><br/>
1. Check if the doctor exists in Identity system using <b>UserManager</b>.<br/>
2. If doctor does not exist → operation is rejected.<br/>
3. Retrieve doctor's profile image path from database.<br/>
4. If profile image exists → delete it from file storage (Supabase).<br/>
5. Delete the doctor from Identity system.<br/><br/>

<b>Important Notes:</b><br/>
- This operation is <b>irreversible</b>.<br/>
- Once deleted, the doctor cannot log in again.<br/>
- Related doctor data linked by foreign keys should be handled carefully (soft delete is recommended for production systems).<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Doctor Deleted Successfully""
}
</pre><br/>

<b>Authentication & Authorization Errors:</b><br/>
- <b>401 Unauthorized</b>:<br/>
  - Missing JWT token.<br/>
  - Invalid or expired JWT token.<br/><br/>

- <b>403 Forbidden</b>:<br/>
  - User is authenticated but does not have <b>Admin</b> role.<br/><br/>

<b>Business Errors (400 Bad Request):</b><br/>
- Doctor user does not exist in the system.<br/>
- Error occurred while deleting the doctor's image.<br/>
- Error occurred while deleting the Identity user.<br/><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Doctor deleted successfully.<br/>
- 400 Bad Request → Business or validation error.<br/>
- 401 Unauthorized → Authentication failure.<br/>
- 403 Forbidden → Authorization failure.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Side Effects:</b><br/>
- Doctor Identity account is permanently removed.<br/>
- Doctor profile image is deleted from storage (if exists).<br/>
";

        #endregion


        #region DeleteReceptionist

        public const string DeleteReceptionistSummary = "Delete a receptionist account";
        public const string DeleteReceptionistDescription =
            @"
<b>Access Control:</b><br/>
- Requires <b>Admin</b> role.<br/>
- Endpoint is protected using <b>JWT Authentication</b>.<br/><br/>

<b>How to send the JWT:</b><br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Purpose:</b><br/>
- Deletes a receptionist account from the system using <b>Soft Delete</b> approach.<br/>
- The receptionist record is not physically removed from the database.<br/>
- Marks the receptionist as deleted to prevent future usage.<br/><br/>

<b>Route Parameters:</b><br/>
- <b>receptionistId</b>:<br/>
  - Required.<br/>
  - Identity User Id of the receptionist to be deleted.<br/><br/>

<b>Business Rules & Logic:</b><br/>
1. Check if the receptionist exists in the database.<br/>
2. If receptionist does not exist → operation is rejected.<br/>
3. Set <b>IsDeleted = true</b> on the receptionist entity.<br/>
4. Save changes to the database.<br/><br/>

<b>Soft Delete Behavior:</b><br/>
- Receptionist data remains in the database for auditing and tracking purposes.<br/>
- Deleted receptionist should be excluded from queries using filters (e.g. IsDeleted = false).<br/>
- Allows safe recovery or historical reference if needed.<br/><br/>

<b>Important Notes:</b><br/>
- This operation does not delete the Identity user unless explicitly handled elsewhere.<br/>
- Recommended to apply global query filters to hide deleted receptionists.<br/>
- Soft delete is safer than hard delete in administrative systems.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""message"": ""Receptionist Deleted Successfully""
}
</pre><br/>

<b>Authentication & Authorization Errors:</b><br/>
- <b>401 Unauthorized</b>:<br/>
  - Missing, invalid, or expired JWT token.<br/><br/>

- <b>403 Forbidden</b>:<br/>
  - User is authenticated but does not have <b>Admin</b> role.<br/><br/>

<b>Business Errors (400 Bad Request):</b><br/>
- Receptionist does not exist in the system.<br/>
- Database save operation failed.<br/><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Receptionist deleted successfully (soft delete).<br/>
- 400 Bad Request → Business error.<br/>
- 401 Unauthorized → Authentication failure.<br/>
- 403 Forbidden → Authorization failure.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Side Effects:</b><br/>
- Receptionist account is logically deleted.<br/>
- Data remains available for auditing and reporting.<br/>
";

        #endregion


        #region GetDashboard

        public const string GetDashboardSummary = "Get admin dashboard statistics";
        public const string GetDashboardDescription =
            @"
<b>Access Control:</b><br/>
- Requires <b>Admin</b> role only.<br/>
- Endpoint is protected using <b>JWT Authentication</b>.<br/><br/>

<b>How to authenticate:</b><br/>
You must send the JWT token in the request header:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Purpose:</b><br/>
- Provides a high-level overview of the system for the Admin dashboard.<br/>
- Used to display statistics, counters, and financial data.<br/><br/>

<b>Returned Dashboard Data:</b><br/>
The response contains the following aggregated values:<br/><br/>

<b>Users & System Metrics:</b><br/>
- <b>TotalPatients</b>: Total number of patients in the system.<br/>
- <b>TotalDoctors</b>: Total number of doctors.<br/>
- <b>TotalReceptionists</b>: Total number of receptionists.<br/>
- <b>TotalAppointments</b>: Total number of appointments created.<br/>
- <b>TotalBills</b>: Total number of bills.<br/>
- <b>TotalCharityNeeds</b>: Total charity requests created.<br/><br/>

<b>Financial Data:</b><br/>
- <b>TotalRevenue</b>: Sum of <b>TotalAmount</b> of all bills with status <b>Paid</b>.<br/>
- <b>PendingBills</b>: Count of bills with status <b>Pending</b>.<br/><br/>

<b>Charity Tracking:</b><br/>
- <b>CompletedCharityNeeds</b>: Number of charity requests marked as completed.<br/><br/>

<b>Business Rules:</b><br/>
- Revenue is calculated only from bills with status <b>Paid</b>.<br/>
- Pending bills are counted separately for monitoring unpaid invoices.<br/>
- Charity completion is based on <b>IsCompleted = true</b>.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
  ""totalPatients"": 1200,
  ""totalDoctors"": 85,
  ""totalReceptionists"": 40,
  ""totalAppointments"": 5200,
  ""totalBills"": 3100,
  ""totalRevenue"": 1250000,
  ""pendingBills"": 420,
  ""totalCharityNeeds"": 180,
  ""completedCharityNeeds"": 95
}
</pre><br/>

<b>Authentication & Authorization Errors:</b><br/>
- <b>401 Unauthorized</b>:<br/>
  - JWT token is missing, invalid, or expired.<br/><br/>

- <b>403 Forbidden</b>:<br/>
  - Authenticated user does not have <b>Admin</b> role.<br/><br/>

<b>Possible HTTP Status Codes:</b><br/>
- 200 OK → Dashboard data returned successfully.<br/>
- 401 Unauthorized → Authentication failed.<br/>
- 403 Forbidden → Authorization failed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/><br/>

<b>Frontend Notes:</b><br/>
- Call this endpoint once when loading the Admin dashboard page.<br/>
- Data is aggregated, no pagination is required.<br/>
- Ideal for charts, KPIs, and summary cards.<br/>
";

        #endregion
    }
}
