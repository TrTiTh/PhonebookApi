using ErrorOr;

namespace PhonebookApi.Errors;

public static class RepositoryErrors
{
    public static class Contacts
    {
        public static Error NotFound => Error.NotFound(
            code: "Contacts.NotFound",
            description: "Contact not found."
        );

        public static Error PhoneNumberConflict => Error.Conflict(
            code: "Contacts.Conflict",
            description: "Phone number is already taken."
        );

        public static Error Unexpected => Error.Unexpected(
            code: "Contacts.Unexpected",
            description: "Something went wrong while processing your request."
        );
    }
}
