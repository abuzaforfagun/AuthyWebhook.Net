namespace AuthyWebhook
{
    public static class Constants
    {
        public const string AUTHY_WEBHOOK_URL = "https://api.authy.com/dashboard/json/application/webhooks";


        public static class Events
        {
            public const string ACCOUNT_RECOVERY_APPROVED = "account_recovery_approved";
            public const string ACCOUNT_RECOVERY_CANCELED = "account_recovery_canceled";
            public const string ACCOUNT_RECOVERY_STARTED = "account_recovery_started";
            public const string CUSTOM_MESSAGE_NOT_ALLOWED = "custom_message_not_allowed";
            public const string DEVICE_REGISTRATION_COMPLETED = "device_registration_completed";
            public const string ONE_TOUCH_REQUEST_RESPONDED = "one_touch_request_responded";
            public const string PHONE_CHANGE_CANCELED = "phone_change_canceled";
            public const string PHONE_CHANGE_PIN_SENT = "phone_change_pin_sent";
            public const string PHONE_CHANGE_REQUESTED = "phone_change_requested";
            public const string PHONE_VERIFICATION_CODE_IS_INVALID = "phone_verification_code_is_invalid";
            public const string PHONE_VERIFICATION_CODE_IS_VALID = "phone_verification_code_is_valid";
            public const string PHONE_VERIFICATION_FAILED = "phone_verification_failed";
            public const string PHONE_VERIFICATION_NOT_FOUND = "phone_verification_not_found";
            public const string PHONE_VERIFICATION_STARTED = "phone_verification_started";
            public const string SUSPENDED_ACCOUNT = "suspended_account";
            public const string TOKEN_INVALID = "token_invalid";
            public const string TOKEN_VERIFIED = "token_verified";
            public const string TOO_MANY_CODE_VERIFICATIONS = "too_many_code_verifications";
            public const string TOO_MANY_PHONE_VERIFICATIONS = "too_many_phone_verifications";
            public const string TOTP_TOKEN_SENT = "totp_token_sent";
            public const string USER_ADDED = "user_added";
            public const string USER_PHONE_CHANGED = "user_phone_changed";
            public const string USER_REMOVED = "user_removed";
        }
    }
}
