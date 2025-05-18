Project Documentation: User Registration and Login Flow
This documentation outlines the key components and functionalities developed for the user registration and login process within the application.
1. Implemented Pages (Views)
LoginorRegister.cshtml (formerly Start.cshtml):
Serves as the entry point for users.
Includes an input field for IC Number and a "LOGIN" button.
Provides a "Register now" link to initiate the account creation process.
Handles displaying "Account not found" messages if the entered IC Number does not exist in the database during login attempt.
AccountCreationInstruction.cshtml:
Displays a list of steps involved in the account creation process.
Includes a "Next" button to proceed.
Features styling with a progress indicator (Step 1 of 4).
CreateAccount.cshtml:
Allows users to enter their Customer Name, IC Number, Mobile Number, and Email Address.
Input fields are marked as required.
Includes client-side JavaScript to enable the "NEXT" button only when all fields are filled.
Features styling with a progress indicator (Step 2 of 4).
UserExistCheck.cshtml:
Displays an "Account already exist" message if a user attempts to create an account with an existing IC Number.
Shows the existing user's details (Name, IC, Mobile, Email).
Provides "RETRY" and "LOGIN" buttons to either return to the Create Account page or proceed to OTP verification.
VerifyOtp.cshtml:
Handles both mobile and email OTP verification.
Displays the OTP for testing purposes (via ViewBag).
Includes four input fields for entering the OTP.
Implements JavaScript for pasting the 4-digit OTP across the input fields.
Includes a "VERIFY" button and a "RESEND" button with a timer.
Displays error messages for incorrect OTP.
PrivacyPolicy.cshtml:
Presents the privacy policy text (placeholder).
Includes an "I agree" checkbox which must be checked to enable the "NEXT" button.
Features styling with a progress indicator (Step 3 of 4).
CreatePin.cshtml:
Prompts the user to create a 6-digit PIN.
Includes six individual input fields for the PIN.
Includes client-side JavaScript to combine the digits and enable the "NEXT" button when all fields are filled.
Features styling with a progress indicator (Step 3 of 4).
ConfirmPin.cshtml:
Prompts the user to confirm their 6-digit PIN.
Includes six individual input fields for the confirmation PIN.
Includes client-side JavaScript to combine the digits and enable the "NEXT" button.
Compares the entered PIN with the initial PIN stored in TempData.
Displays an "Unmatched PIN" error message if the PINs do not match.
Features styling with a progress indicator (Step 4 of 4).
BiometricLogin.cshtml:
Provides an option to enable biometric login (Face ID/Fingerprint icons included as placeholders).
Includes "ENABLE NOW" and "MAYBE LATER" buttons.
Features styling with a progress indicator (Step 4 of 4).
AccountCreationSuccess.cshtml:
Represents the user's landing page after successful account creation/login.
Displays a "Hello, [Customer Name]" greeting, where the name is retrieved from TempData.
Includes placeholder content for personal finance and cashback information.
Features a placeholder for a bottom navigation bar.
2. Controller (RegistrationController.cs)
Handles the navigation flow between the different registration and login steps.
Start() (GET): Renders the LoginorRegister.cshtml view.
Start(string icNumber) (POST):
Receives the IC Number from the login form.
Queries the database (_dbContext.Users) to check if a user with the provided IC Number exists.
If the user exists, redirects to the VerifyOtp action.
If the user does not exist, adds a model error and returns the LoginorRegister view to display the "Account not found" message.
AccountCreationInstruction() (GET): Renders the AccountCreationInstruction.cshtml view.
CreateAccount() (GET): Renders the CreateAccount.cshtml view.
CreateAccount(RegistrationCreateAccountViewModel model) (POST):
Receives user details from the Create Account form.
Checks if a user with the entered IC Number already exists in the database.
If an existing user is found, creates a RegistrationCreateAccountViewModel with the existing user's data and redirects to the UserExistCheck action.
If the user does not exist, proceeds to generate an OTP, stores user data and OTP in TempData and Session, and redirects to the VerifyOtp action.
UserExistCheck(RegistrationCreateAccountViewModel userModel) (GET): Receives user data and renders the UserExistCheck.cshtml view.
HandleUserExistChoice(string choice, string icNumber) (POST):
Receives the user's choice ("retry" or "login") from the UserExistCheck form.
If "login", redirects to the VerifyOtp action with the IC Number.
If "retry", redirects to the CreateAccount action.
VerifyOtp(string icNumber) (GET): Renders the VerifyOtp.cshtml view, retrieves user data and OTP from TempData/Session, and passes relevant data to the view.
VerifyOtp(RegistrationVerifyOtpViewModel model) (POST):
Validates the entered OTP against the one stored in Session.
If valid, removes the OTP from Session and redirects to VerifyEmailOtp.
If invalid, adds a model error and returns the VerifyOtp view.
VerifyEmailOtp(string icNumber, string email) (GET): Generates an email OTP, stores it in Session, and renders the VerifyOtp.cshtml view (reusing the same view with different data).
VerifyEmailOtp(RegistrationVerifyOtpViewModel model) (POST): Validates the email OTP. If valid, removes the OTP from Session and redirects to PrivacyPolicy.
PrivacyPolicy() (GET): Renders the PrivacyPolicy.cshtml view.
PrivacyPolicy(RegistrationPrivacyPolicyViewModel model) (POST): Validates the privacy policy acceptance and redirects to CreatePin.
CreatePin() (GET): Renders the CreatePin.cshtml view.
CreatePin(RegistrationCreatePinViewModel model) (POST): Validates the PIN, stores it in TempData["InitialPin"], and redirects to ConfirmPin.
ConfirmPin() (GET): Renders the ConfirmPin.cshtml view, ensuring the initial PIN is available in TempData.
ConfirmPin(RegistrationConfirmPinViewModel model) (POST):
Retrieves the initial PIN from TempData.
Compares the confirmed PIN with the initial PIN.
If they match, redirects to BiometricLogin.
If they don't match, adds a model error and returns the ConfirmPin view.
BiometricLogin() (GET): Renders the BiometricLogin.cshtml view.
HandleBiometricChoice(string choice) (POST):
Retrieves the user data (RegistrationCreateAccountViewModel) and the InitialPin from TempData.
Includes placeholder logic to save the user data to the database.
SECURITY WARNING: Currently saves the PIN as clear text. This should be replaced with secure hashing and salting.
Stores the CustomerName in TempData for the success page.
Clears other user-specific data from TempData.
Redirects to the AccountCreationSuccess action.
AccountCreationSuccess() (GET): Renders the AccountCreationSuccess.cshtml view, which displays the customer name from TempData.
3. Models (RegistrationViewModels.cs & User.cs)
RegistrationStartViewModel: Used for the initial login/register page (currently just a placeholder).
RegistrationVerifyOtpViewModel: Used for mobile and email OTP verification.
RegistrationPrivacyPolicyViewModel: Used for the privacy policy acceptance.
RegistrationCreatePinViewModel: Used for creating the initial PIN.
RegistrationConfirmPinViewModel: Used for confirming the PIN.
RegistrationCreateAccountViewModel: Used for collecting user details during account creation and displaying existing user details.
User (in Data.DbModels): Represents the user entity in the database. Includes properties for user details and PasswordHash. Note: The current implementation saves the clear text PIN to PasswordHash which is a security risk and should be updated.
This documentation covers the main components and their interactions in the implemented flow.