﻿namespace Goodpets.Domain.Users.Identities;

public record JwtId : Identity
{
    public JwtId(Guid value) : base(value)
    {
    }

    public JwtId()
    {
    }

    public static implicit operator JwtId(string jwtId)
    {
        if (string.IsNullOrEmpty(jwtId))
            throw new ArgumentNullException(nameof(jwtId));

        return new JwtId(Guid.Parse(jwtId));
    }
}