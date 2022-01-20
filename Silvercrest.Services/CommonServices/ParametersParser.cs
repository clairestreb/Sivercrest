namespace Silvercrest.Services.CommonServices
{
    public static class ParametersParser
    {
        public static bool? GetBoolValue(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return null;
            }
            bool value;
            var result = bool.TryParse(parameter, out value);
            if (result)
            {
                return value;
            }
            return null;
        }

        public static int? GetIntValue(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return null;
            }
            int value;
            var result = int.TryParse(parameter, out value);
            if (result)
            {
                return value;
            } 
            return null;
        }
    }
}