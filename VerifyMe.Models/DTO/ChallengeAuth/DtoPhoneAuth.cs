﻿namespace VerifyMe.Models.DTO.ChallengeAuth;

public class DtoPhoneAuth
{
    public string Phone { get; set; } = string.Empty;

    public DtoPhoneAuth()
    {
        
    }

    public DtoPhoneAuth(string phone)
    {
        Phone = phone;
    }
}