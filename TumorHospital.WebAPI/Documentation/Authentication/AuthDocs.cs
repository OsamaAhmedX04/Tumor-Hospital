namespace TumorHospital.WebAPI.Documentation.Authentication
{
    public static class AuthDocs
    {
        #region Register

        public const string RegisterSummary = "Register a new patient account";
        public const string RegisterDescription =
            @"
<b>Purpose:</b><br/>
- Allows a new patient to create an account in the system.<br/>
- Sends an email confirmation token upon successful registration.<br/><br/>

<b>Who can access:</b><br/>
- Anonymous users (not logged in).<br/><br/>

<b>Request Body (RegisterDto):</b><br/>
- <b>FirstName</b> (string, required): Patient's first name.<br/>
  - Must be between 2 and 20 characters.<br/>
- <b>LastName</b> (string, required): Patient's last name.<br/>
  - Must be between 2 and 20 characters.<br/>
- <b>Email</b> (string, required): Patient's email address.<br/>
  - Cannot be empty.<br/>
- <b>Password</b> (string, required): Account password.<br/>
  - Cannot be empty.<br/>
- <b>Gender</b> (string, required): Patient's gender.<br/>
  - Must be either 'Male' or 'Female'.<br/><br/>

<b>Validation Rules:</b><br/>
- FirstName: required, 2–20 characters.<br/>
- LastName: required, 2–20 characters.<br/>
- Email: required, must be valid email format.<br/>
- Password: required, must satisfy complexity policy.<br/>
- Gender: required, only 'Male' or 'Female'.<br/><br/>

<b>Business Logic:</b><br/>
1. Create a new ApplicationUser.<br/>
2. Assign the role <b>Patient</b>.<br/>
3. Create a Patient record linked to the user.<br/>
4. Generate a 6-digit email confirmation token.<br/>
5. Set token expiration to 1 hour.<br/>
6. Send confirmation email to the patient.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""User Registered Successfully, We Sent Email Confirmation""
}
</pre><br/>

<b>Possible Errors (400 Bad Request):</b><br/>
- Validation errors:<br/>
<pre>
{
    ""Errors"": {
        ""FirstName"": [""First Name Is Required"", ""First Name Should be between 2 and 20 letter""],
        ""LastName"": [""Last Name Is Required"", ""Last Name Should be between 2 and 20 letter""],
        ""Email"": [""Email Is Required""],
        ""Password"": [""Password Is Required""],
        ""Gender"": [""Gender Is Required"", ""Invalid Gender Value. Only Male Or Female""]
    }
}
</pre>
- Identity errors (e.g., email already exists):<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""Email 'test@example.com' is already taken.""]
    }
}
</pre><br/>

<b>Exceptions:</b><br/>
- Any unexpected errors during user creation will return as 400 with Identity error details.<br/><br/>

<b>Side Effects:</b><br/>
- Patient record created in the database.<br/>
- Email confirmation sent.<br/>
- Token saved in UserTokens table with 1-hour expiry.<br/>
- Role assigned as <b>Patient</b>.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Registration successful.<br/>
- 400 Bad Request → Validation failed or Identity errors.<br/>
- 500 Internal Server Error → Unexpected errors (rare).<br/>
";

        #endregion


        #region ConfirmEmail

        public const string ConfirmEmailSummary = "Confirm patient's email and activate account";
        public const string ConfirmEmailDescription =
            @"
<b>Purpose:</b><br/>
- Confirms a newly registered patient's email using a token.<br/>
- Activates the patient account.<br/>
- Generates JWT & Refresh Token for authenticated sessions.<br/><br/>

<b>Who can access:</b><br/>
- Anonymous users (only the user who received the confirmation email).<br/><br/>

<b>Request Body (ConfirmEmailDto):</b><br/>
- <b>Email</b> (string, required): The email used during registration.<br/>
- <b>Token</b> (string, required): 6-digit email confirmation token received via email.<br/><br/>

<b>Validation Rules:</b><br/>
- Email must exist in the system.<br/>
- Token must match the saved token.<br/>
- Token must not be expired (valid for 1 hour after issuance).<br/><br/>

<b>Business Logic:</b><br/>
1. Find the user by email.<br/>
2. Check if email is already confirmed → throw exception if yes.<br/>
3. Retrieve the saved email confirmation token.<br/>
4. Validate token and expiration → throw exception if invalid or expired.<br/>
5. Set <b>EmailConfirmed</b> and <b>IsActive</b> to true.<br/>
6. Remove the used authentication token.<br/>
7. Retrieve user roles.<br/>
8. Generate JWT token and refresh token.<br/>
9. Store or update refresh token in database with 20-day expiration.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""Your Email Confirmed Successfully"",
    ""UserId"": ""GUID"",
    ""Token"": ""JWT Token"",
    ""RefreshToken"": ""Refresh Token""
}
</pre><br/>

<b>Possible Errors (400 Bad Request):</b><br/>
- User not found:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""User Not Found""]
    }
}
</pre>
- Email already confirmed:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""Email Is Already Confirmed Before""]
    }
}
</pre>
- Invalid or expired token:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""Invalid Or Expired Token""]
    }
}
</pre><br/>

<b>Exceptions:</b><br/>
- Any unexpected errors during email confirmation or token generation will return 400 with Identity error details.<br/><br/>

<b>Side Effects:</b><br/>
- User's <b>EmailConfirmed</b> and <b>IsActive</b> flags updated.<br/>
- Confirmation token removed from database.<br/>
- JWT token & Refresh token generated and saved.<br/>
- User can now log in with email and password.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Email confirmed successfully.<br/>
- 400 Bad Request → User not found, token invalid/expired, or already confirmed.<br/>
- 500 Internal Server Error → Unexpected errors (rare).<br/>
";

        #endregion


        #region ResetConfirmEmailToken

        public const string ResetConfirmEmailTokenSummary = "Resend email confirmation token to the user";
        public const string ResetConfirmEmailTokenDescription =
            @"
<b>Purpose:</b><br/>
- Allows a user to request a new email confirmation token if they haven't confirmed their email yet.<br/>
- Sends a new 6-digit confirmation token to the user's email.<br/><br/>

<b>Who can access:</b><br/>
- Anonymous users who registered but haven't confirmed their email.<br/><br/>

<b>Request Body (EmailDto):</b><br/>
- <b>Email</b> (string, required): The email used during registration.<br/><br/>

<b>Validation Rules:</b><br/>
- Email must exist in the system.<br/>
- Email must not be already confirmed.<br/><br/>

<b>Business Logic:</b><br/>
1. Find the user by email.<br/>
2. Check if the email is already confirmed → throw exception if yes.<br/>
3. Generate a new 6-digit confirmation token.<br/>
4. Save the token in UserTokens table with 1-hour expiry.<br/>
5. Send the token via email using EmailService.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""Confirm Email Token Resent To Your Email""
}
</pre><br/>

<b>Possible Errors (400 Bad Request):</b><br/>
- User not found:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""User Not Exist""]
    }
}
</pre>
- Email already confirmed:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""This Email Is Already Confirmed Before""]
    }
}
</pre><br/>

<b>Exceptions:</b><br/>
- Any unexpected errors during token generation or email sending will return 400 with Identity error details.<br/><br/>

<b>Side Effects:</b><br/>
- New token saved in UserTokens table with 1-hour expiration.<br/>
- Confirmation email sent to the user.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Token resent successfully.<br/>
- 400 Bad Request → User not found or email already confirmed.<br/>
- 500 Internal Server Error → Unexpected errors (rare).<br/>
";

        #endregion


        #region Login

        public const string LoginSummary = "Authenticate a user and generate JWT & Refresh Token";
        public const string LoginDescription =
            @"
<b>Purpose:</b><br/>
- Authenticates a registered user (patient, doctor, receptionist, or admin).<br/>
- Generates JWT token and Refresh token for session management.<br/>
- Activates the user account if login is successful.<br/><br/>

<b>Who can access:</b><br/>
- Anonymous users who already have registered and confirmed their email.<br/><br/>

<b>Request Body (LoginDto):</b><br/>
- <b>Email</b> (string, required): The email used during registration.<br/>
  - Cannot be empty.<br/>
- <b>Password</b> (string, required): User's password.<br/>
  - Cannot be empty.<br/><br/>

<b>Validation Rules:</b><br/>
- Email: required.<br/>
- Password: required.<br/><br/>

<b>Business Logic:</b><br/>
1. Find the user by email.<br/>
2. Check if the user exists → throw exception if not.<br/>
3. Verify that the email is confirmed → throw exception if not.<br/>
4. Check password → throw exception if incorrect.<br/>
5. Retrieve user roles.<br/>
6. Generate JWT token and Refresh token.<br/>
7. Save or update Refresh token in the database with 20-day expiration.<br/>
8. Set user's <b>IsActive</b> flag to true.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""Login Successfully"",
    ""UserId"": ""GUID"",
    ""Token"": ""JWT Token"",
    ""RefreshToken"": ""Refresh Token""
}
</pre><br/>

<b>Possible Errors (400 Bad Request):</b><br/>
- User not found:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""User Not Found""]
    }
}
</pre>
- Email not confirmed:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""Email Not Confirmed Yet""]
    }
}
</pre>
- Incorrect password:<br/>
<pre>
{
    ""Errors"": {
        ""Identity"": [""Email Or Password Wrong""]
    }
}
</pre>
- Validation errors (empty email or password):<br/>
<pre>
{
    ""Errors"": {
        ""Email"": [""Email Is Required""],
        ""Password"": [""Password Is Required""]
    }
}
</pre><br/>

<b>Exceptions:</b><br/>
- Any unexpected errors during authentication will return 400 with Identity error details.<br/><br/>

<b>Side Effects:</b><br/>
- JWT token and Refresh token created and saved.<br/>
- User's <b>IsActive</b> flag set to true.<br/>
- User can now make authenticated requests with JWT.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Login successful.<br/>
- 400 Bad Request → Validation failed, user not found, email not confirmed, or incorrect password.<br/>
- 500 Internal Server Error → Unexpected errors (rare).<br/>
";

        #endregion


        #region Logout

        public const string LogoutSummary = "Logout the currently authenticated user";
        public const string LogoutDescription =
            @"
<b>Purpose:</b><br/>
- Logs out the currently authenticated user.<br/>
- Invalidates the refresh token so the user cannot get a new JWT without logging in again.<br/><br/>

<b>Authentication Required:</b><br/>
- <b>This endpoint is protected.</b><br/>
- You <b>must</b> send a valid JWT token in the request header.<br/><br/>

<b>How to send the JWT:</b><br/>
- Add the token to the <b>Authorization</b> header like this:<br/>
<pre>
Authorization: Bearer &lt;JWT_TOKEN&gt;
</pre><br/>

<b>Who can access:</b><br/>
- Authenticated users with active roles only:<br/>
  <b>Doctor, Patient, Admin</b>.<br/><br/>

<b>Request Parameters:</b><br/>
- <b>userId</b> (string, required): The ID of the currently logged-in user.<br/><br/>

<b>Business Logic:</b><br/>
1. Find the refresh token related to the given userId.<br/>
2. If found, delete it from the database.<br/>
3. Save changes.<br/><br/>

<b>Success Response (200 OK):</b><br/>
<pre>
{
    ""Message"": ""Loged Out""
}
</pre><br/>

<b>Authentication Errors:</b><br/>
- <b>401 Unauthorized</b>:<br/>
  - JWT token is missing.<br/>
  - JWT token is invalid or expired.<br/><br/>

- <b>403 Forbidden</b>:<br/>
  - JWT token is valid, but the user role is not allowed to access this endpoint.<br/><br/>

<b>Other Errors:</b><br/>
- <b>500 Internal Server Error</b>:<br/>
  - Unexpected error while deleting refresh token.<br/><br/>

<b>Side Effects:</b><br/>
- Refresh token is removed from the database.<br/>
- User must login again to obtain a new JWT.<br/><br/>

<b>HTTP Status Codes:</b><br/>
- 200 OK → Logout successful.<br/>
- 401 Unauthorized → Missing / invalid / expired JWT.<br/>
- 403 Forbidden → User role not allowed.<br/>
- 500 Internal Server Error → Unexpected server error.<br/>
";

        #endregion
    }
}
