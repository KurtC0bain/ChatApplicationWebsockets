namespace ChatApplicationWebsockets.Exceptions
{
    [Serializable]
    class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException() { }

        public UserAlreadyExistException(string name)
            : base($"\"{name}\" name is already taken.\nPlease, choose another name") {}
    }
}
