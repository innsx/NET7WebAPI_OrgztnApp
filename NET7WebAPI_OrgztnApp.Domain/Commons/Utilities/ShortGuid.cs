namespace NET7WebAPI_OrgztnApp.Domain.Commons.Utilities
{
    public class ShortGuid
    {
        public static string NewGuid()
        {
            var guid = Guid.NewGuid();

            //Converting guid into bytes of array
            byte[] bytesArray = guid.ToByteArray();

            string oldValue = "/";
            string newValue = "_";

            string oldValue1 = "+";
            string newValue1 = "-";

            int startsAt = 0;
            int endsAt = 22;

            string base64String = Convert.ToBase64String(bytesArray)
                                        .Replace(oldValue, newValue)
                                        .Replace(oldValue1, newValue1)
                                        .Substring(startsAt, endsAt);

            return base64String;
        }
    }
}
