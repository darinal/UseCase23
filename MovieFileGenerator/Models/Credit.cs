﻿namespace MovieFileGenerator.Models;

public record Credit
{
    public int Id { get; set; }
    public int TitleId { get; set; }
    public string? RealName { get; set; }
    public string? CharacterName { get; set; }
    public string? Role { get; set; }
}