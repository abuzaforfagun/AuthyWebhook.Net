# AuthyWebhook.Net

This repo demonstrates how to Create, List, Delete Authy webhooks. You can set a webhook to be called after a publically visible event (listed below). Webhooks use a POST when responding. This code also demonstrates how to sign a request and verify the signature in a response.

Official documentation may be found here: https://www.twilio.com/docs/api/authy/authy-webhooks-api

### Setup Environment
1. Install nuget package or clone this repository and add refference of AuthyWebhook project.
2. Browse to the application you want to use in the twilio.com/console where (once enabled) you should now see:
    * App API Key
    * Your Access Key
    * API Signing Key
    * Authy API Key
3. Create authy object of AuthyConfiguration with you api key, access key and signin key.  var authyConfiguration = new AuthyConfiguration("API_KEY", "ACCESS_KEY", "SIGNIN_KEY"); 
4. Create object of AuthyWebHookHelper.  new AuthyWebHookHelper(authyConfiguration); 

## Public Webhook Events
You can trigger webhooks using the following events.  You can use multiple events in a single webhook.

* account_recovery_approved
* account_recovery_canceled
* account_recovery_started
* custom_message_not_allowed
* device_registration_completed
* one_touch_request_responded
* phone_change_canceled
* phone_change_pin_sent
* phone_change_requested
* phone_verification_code_is_invalid
* phone_verification_code_is_valid
* phone_verification_failed
* phone_verification_not_found
* phone_verification_started
* suspended_account
* token_invalid
* token_verified
* too_many_code_verifications
* too_many_phone_verifications
* totp_token_sent
* user_added
* user_phone_changed
* user_removed

### Listing Webhooks
authyWebHookHelper.Get<T>();

### Create a Webhook
use string or Response as value of T
 var webHook = new WebHook("one_touch_request_responded", Constants.Events.ONE_TOUCH_REQUEST_RESPONDED, "https://example/api/webhooked");
            var result = authyWebHookHelper.Create<T>(webHook);
### Deleting a Webhook
 authyWebHookHelper.Delete("WEB_HOOK_ID"); 