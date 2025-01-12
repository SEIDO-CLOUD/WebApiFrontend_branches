    namespace Models;

    public class AdminInfo
    {
        public string AppEnvironment {get; set;}
        public string SecretSource {get; set;}
        public string DataConnectionTag {get; set;}
        public string DefaultDataUser {get; set;}
        public string MigrationDataUser {get; set;}
        public string DataConnectionServerString {get; set;} 
    }