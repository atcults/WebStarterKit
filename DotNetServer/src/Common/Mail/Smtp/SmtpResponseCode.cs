namespace Common.Mail.Smtp
{
    /// Specify smtp command result code.
    /// <summary>
    /// Specify smtp command result code.
    /// </summary>
    public enum SmtpCommandResultCode
    {
		/// <summary>
		/// 
		/// </summary>
        None = 0,
        /// 211: The system state. Help system ready.
        /// <summary>
        /// 211: The system state. Help system ready.
        /// </summary>
        SystemStatusOrSystemHelpReply = 211,
        /// 214: the help message. 
        /// <summary>
        /// 214: the help message. 
        /// </summary>
        HelpMessage = 214,
        /// 220: Ready. 
        /// <summary>
        /// 220: Ready. 
        /// </summary>
        ServiceReady = 220,
        /// 221: To close the connection.
        /// <summary>
        /// 221: To close the connection.
        /// </summary>
        ServiceClosingTransmissionChannel = 221,
        /// 235: The authentication is successful.
        /// <summary>
        /// 235: The authentication is successful.
        /// </summary>
        AuthenticationSuccessful = 235,
        /// 250: The requested operation can be performed. Complete.
        /// <summary>
        /// 250: The requested operation can be performed. Complete.
        /// </summary>
        RequestedMailActionOkayCompleted = 250,
        /// 251: the recipient because it no longer exists in the forward-path you want to transfer to. 
        /// <summary>
        /// 251: the recipient because it no longer exists in the forward-path you want to transfer to. 
        /// </summary>
        UserNotLocalWillForwardTo = 251,
        /// 252: Failed to verify the user. But the message delivery is received.
        /// <summary>
        /// 252: Failed to verify the user. But the message delivery is received.
        /// </summary>
        CannotVerifyUserButWillAcceptMessageAndAttemptDelivery = 252,
        /// 334: The authentication process while waiting.
        /// <summary>
        /// 334: The authentication process while waiting.
        /// </summary>
        WaitingForAuthentication= 334,
        /// 354 of the e-mails start type. End of input is " ." To send the line only. 
        /// <summary>
        /// 354 of the e-mails start type. End of input is " ." To send the line only. 
        /// </summary>
        StartMailInput = 354,
        /// 421: The service is not available. To close the connection. 
        /// <summary>
        /// 421: The service is not available. To close the connection. 
        /// </summary>
        ServiceNotAvailableClosingTransmissionChannel = 421,
        /// 432: Password must be changed.
        /// <summary>
        /// 432: Password must be changed.
        /// </summary>
        APasswordTransitionIsNeeded = 432,
        /// 450: mailbox is not available for the requested operation is not possible.
        /// <summary>
        /// 450: mailbox is not available for the requested operation is not possible.
        /// </summary>
        RequestedMailActionNotTakenMailboxUnavailable = 450,
        /// 451: error occurs during processing. Requested operation has failed.
        /// <summary>
        /// 451: error occurs during processing. Requested operation has failed.
        /// </summary>
        RequestedActionAbortedErrorInProcessing = 451,
        /// 454: a temporary authentication failed.
        /// <summary>
        /// 454: a temporary authentication failed.
        /// </summary>
        TemporaryAuthenticationFailure = 454,
        /// 500: Grammar there is an error in the command because you don't understand.
        /// <summary>
        /// 500: Grammar there is an error in the command because you don't understand.
        /// </summary>
        SyntaxErrorCommandUnrecognized = 500,
        /// 501: There is an error in the syntax of the arguments.
        /// <summary>
        /// 501: There is an error in the syntax of the arguments.
        /// </summary>
        SyntaxErrorInParametersOrArguments = 501,
        /// 502: The command is in this system is not implemented.
        /// <summary>
        /// 502: The command is in this system is not implemented.
        /// </summary>
        CommandNotImplemented = 502,
        /// 503: Command Issued order is wrong.
        /// <summary>
        /// 503: Command Issued order is wrong.
        /// </summary>
        BadSequenceOfCommands = 503,
        /// 504: The command argument is undefined.
        /// <summary>
        /// 504: The command argument is undefined.
        /// </summary>
        CommandParameterNotImplemented = 504,
        /// 530: The authentication is required.
        /// <summary>
        /// 530: The authentication is required.
        /// </summary>
        AuthenticationRequired = 530,
        /// 538: The requested authentication process will need to be encrypted.
        /// <summary>
        /// 538: The requested authentication process will need to be encrypted.
        /// </summary>
        EncryptionRequiredForRequestedAuthenticationMechanism = 538,
        /// 550: mailbox is not available for the requested operation is not possible. 
        /// <summary>
        /// 550: mailbox is not available for the requested operation is not possible. 
        /// </summary>
        RequestedActionNotTakenMailboxUnavailable = 550,
        /// 551: the recipient does not exist. The forward-path to send out.
        /// <summary>
        /// 551: the recipient does not exist. The forward-path to send out.
        /// </summary>
        UserNotLocalPleaseTry = 551,
        /// 552: Out of disk space because of the requested operation is not possible.
        /// <summary>
        /// 552: Out of disk space because of the requested operation is not possible.
        /// </summary>
        RequestedMailActionAbortedExceededStorageAllocation = 552,
        /// 553: mailbox is not allowed for the requested operation is not performed.
        /// <summary>
        /// 553: mailbox is not allowed for the requested operation is not performed.
		/// </summary>
        RequestedActionNotTakenMailboxNameNotAllowed = 553,
        /// 554: The process failed.
        /// <summary>
        /// 554: The process failed.
        /// </summary>
        TransactionFailed = 554
    }
}
