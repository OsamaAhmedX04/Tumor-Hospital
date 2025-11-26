using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Domain.Constants
{
    public static class EmailBody
    {
        public static string GetDoctorEmailCreatedBody(string firstName, string lastName, string doctorPassword)
        {
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                    <h2 style='text-align: center; color: #333;'>Welcome to Our Hospital!</h2>

                    <p style='font-size: 15px; color: #555;'>
                        Dear Dr. {firstName} {lastName},<br/><br/>
                        Your account has been successfully created. Below is your temporary password to access your account:
                    </p>

                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='display: inline-block; background-color: #28a745; color: white; padding: 14px 28px; font-size: 22px; letter-spacing: 2px; border-radius: 6px;'>
                            {doctorPassword}
                        </span>
                    </div>

                    <p style='font-size: 14px; color: #666;'>
                        ⚠️ For security reasons, please <strong>do not share this password with anyone</strong>.<br/>
                        You are required to change it after your first login.
                    </p>

                    <p style='margin-top: 30px; font-size: 14px; color: #333;'>
                        Best regards,<br/>
                        <strong>The Hospital Admin Team</strong>
                    </p>
                </div>
                ";
            return body;
        }
        public static string GetReceptionistEmailCreatedBody(string firstName, string lastName, string receptionistPassword)
        {
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                    <h2 style='text-align: center; color: #333;'>Welcome to Our Hospital Team!</h2>

                    <p style='font-size: 15px; color: #555;'>
                        Dear {firstName} {lastName},<br/><br/>
                        We’re pleased to inform you that your <strong>Receptionist account</strong> has been successfully created.<br/>
                        You can now log in to the system using the temporary password below:
                    </p>

                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='display: inline-block; background-color: #007bff; color: white; padding: 14px 28px; font-size: 22px; letter-spacing: 2px; border-radius: 6px;'>
                            {receptionistPassword}
                        </span>
                    </div>

                    <p style='font-size: 14px; color: #666;'>
                        ⚠️ For security reasons, please <strong>do not share this password with anyone</strong>.<br/>
                        You are required to change it after your first login.
                    </p>

                    <p style='margin-top: 30px; font-size: 14px; color: #333;'>
                        Best regards,<br/>
                        <strong>The Hospital Admin Team</strong>
                    </p>
                </div>
            ";
            return body;
        }
        public static string GetPatientEmailCreatedBody(string token)
        {
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                    <h2 style='text-align: center; color: #333;'>Confirm Your Email</h2>

                    <p style='font-size: 15px; color: #555;'>
                        Thanks for registering! Please use the code below to confirm your email:
                    </p>

                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='display: inline-block; background-color: #007bff; color: white; padding: 14px 28px; font-size: 22px; letter-spacing: 3px; border-radius: 6px;'>
                            {token}
                        </span>
                    </div>

                    <p style='font-size: 14px; color: #666;'>
                        If you didn’t create this account, you can ignore this email.
                    </p>

                    <p style='margin-top: 30px; font-size: 14px; color: #333;'>
                        Best regards,<br/>
                        <strong>Your App Team</strong>
                    </p>
                </div>
                ";
            return body;
        }
    }
}
