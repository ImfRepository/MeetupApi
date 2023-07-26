﻿namespace Meetup.Core.Domain.Exceptions;

public class RepositoryException : Exception
{
    public RepositoryException(string message)
        : base(message)
    {

    }

    public RepositoryException(string message, Exception exception)
	    : base(message, exception)
    {

    }
}