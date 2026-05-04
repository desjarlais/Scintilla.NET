using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiToolkit;

public static class ApiTools
{
    private static string NullifyEmpty(string s) => string.IsNullOrEmpty(s) ? null : s;

    private static long? ParseValue(string text, out bool isHex)
    {
        isHex = text.StartsWith("0x");
        return isHex ?
            (long.TryParse(text.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out long result) ? result : null) :
            (long.TryParse(text, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result) ? result : null);
    }

    public static IEnumerable<ApiFeature> Read(TextReader input)
    {
        StringBuilder comment = new();
        string category = null;
        List<ApiEnum> groups = [];

        Regex catRegex = new(@"^(?<type>cat) (?<name>\w+)$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        Regex enuRegex = new(@"^(?<type>enu) (?<name>\w+)=(?<prefix>\w+)( (?<prefix>\w+))*$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        Regex lexRegex = new(@"^(?<type>lex) (?<name>\w+)=(?<lexerName>\w+) (?<prefix>\w+)( (?<prefix>\w+))*$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        Regex valRegex = new(@"^(?<type>val) (?<name>\w+)=(?<value>(0x[0-9A-Fa-f]+|-?\d+))$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        Regex funRegex = new(@"^(?<type>fun|get|set) (?<returnType>\w+) (?<name>\w+)(=(?<value>(0x[0-9A-Fa-f]+|-?\d+)))?\(((?<wparamType>\w+) (?<wparamName>\w+)(=(?<wparamDefault>0x[0-9A-Fa-f]+|-?\d+))?)?, *((?<lparamType>\w+) (?<lparamName>\w+)(=(?<lparamDefault>0x[0-9A-Fa-f]+|-?\d+))?)?\)$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        Regex evtRegex = new(@"^(?<type>evt) (?<returnType>\w+) (?<name>\w+)(=(?<value>(0x[0-9A-Fa-f]+|-?\d+)))?\((void|(?<params>.*?))\)$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        Regex paramRegex = new(@"^(?<paramType>\w+) (?<paramName>\w+)(=(?<paramDefault>0x[0-9A-Fa-f]+|-?\d+))?$",
            RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

        for (string line = input.ReadLine(); line != null; line = input.ReadLine())
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                comment.Clear();
                continue;
            }

            if (line.StartsWith("##") || line.StartsWith("#!"))
                continue;

            if (line.StartsWith("# "))
            {
                if (comment.Length > 0)
                    comment.AppendLine();
                comment.Append(line.Trim(), 2, line.Length - 2);
            }
            else
            {
                {
                    var match = catRegex.Match(line);
                    if (match.Success)
                    {
                        category = match.Groups["name"].Value;
                        comment.Clear();
                        continue;
                    }
                }
                {
                    var match = enuRegex.Match(line);
                    if (match.Success)
                    {
                        string type = match.Groups["type"].Value;
                        string name = match.Groups["name"].Value;
                        string[] prefixes = match.Groups["prefix"].Captures.Cast<Capture>().Select(x => x.Value).ToArray();
                        var apiEnum = new ApiEnum() {
                            Type = type,
                            Name = name,
                            Category = category,
                            Comment = comment.ToString(),
                            Prefixes = prefixes,
                        };
                        groups.Add(apiEnum);
                        comment.Clear();
                        continue;
                    }
                }
                {
                    var match = lexRegex.Match(line);
                    if (match.Success)
                    {
                        string type = match.Groups["type"].Value;
                        string name = match.Groups["name"].Value;
                        string lexerName = match.Groups["lexerName"].Value;
                        string[] prefixes = match.Groups["prefix"].Captures.Cast<Capture>().Select(x => x.Value).ToArray();
                        var apiLexer = new ApiLexer() {
                            Type = type,
                            Name = name,
                            Category = category,
                            Comment = comment.ToString(),
                            LexerName = lexerName,
                            Prefixes = prefixes,
                        };
                        groups.Add(apiLexer);
                        comment.Clear();
                        continue;
                    }
                }
                {
                    var match = valRegex.Match(line);
                    if (match.Success)
                    {
                        string type = match.Groups["type"].Value;
                        string name = match.Groups["name"].Value;
                        string rawValue = match.Groups["value"].Value;
                        long? value = ParseValue(rawValue, out bool isHex);

                        bool found = false;

                        if (!found)
                        {
                            ApiLexer[] lexers = groups.Where(g => g.Prefixes.Any(prefix => name.StartsWith(prefix))).OfType<ApiLexer>().ToArray();
                            if (lexers.Length > 0)
                            {
                                yield return new ApiLexerValue() {
                                    Type = type,
                                    Name = name,
                                    Category = category,
                                    Comment = comment.ToString(),
                                    RawValue = NullifyEmpty(rawValue),
                                    Value = value,
                                    IsHexValue = isHex,
                                    Lexers = lexers,
                                };
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            ApiEnum[] enums = groups.Where(g => g.Prefixes.Any(prefix => name.StartsWith(prefix))).ToArray();
                            if (enums.Length > 0)
                            {
                                yield return new ApiEnumValue() {
                                    Type = type,
                                    Name = name,
                                    Category = category,
                                    Comment = comment.ToString(),
                                    RawValue = NullifyEmpty(rawValue),
                                    Value = value,
                                    IsHexValue = isHex,
                                    Enums = enums,
                                };
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            yield return new ApiValue() {
                                Type = type,
                                Name = name,
                                Category = category,
                                Comment = comment.ToString(),
                                RawValue = NullifyEmpty(rawValue),
                                Value = value,
                                IsHexValue = isHex,
                            };
                        }

                        comment.Clear();
                        continue;
                    }
                }
                {
                    var match = funRegex.Match(line);
                    if (match.Success)
                    {
                        string type = match.Groups["type"].Value;
                        string name = match.Groups["name"].Value;
                        string rawValue = match.Groups["value"].Value;
                        long? value = ParseValue(rawValue, out bool isHex);
                        string returnType = match.Groups["returnType"].Value;
                        string wparamType = match.Groups["wparamType"].Value;
                        string wparamName = match.Groups["wparamName"].Value;
                        string wparamDefault = match.Groups["wparamDefault"].Value;
                        string lparamType = match.Groups["lparamType"].Value;
                        string lparamName = match.Groups["lparamName"].Value;
                        string lparamDefault = match.Groups["lparamDefault"].Value;

                        yield return new ApiFunction() {
                            Type = type,
                            Name = name,
                            Category = category,
                            Comment = comment.ToString(),
                            RawValue = NullifyEmpty(rawValue),
                            Value = value,
                            IsHexValue = isHex,
                            ReturnType = returnType,
                            Wparam = string.IsNullOrEmpty(wparamName) ? null : new ApiParam() {
                                Type = NullifyEmpty(wparamType),
                                Name = NullifyEmpty(wparamName),
                                Default = NullifyEmpty(wparamDefault),
                            },
                            Lparam = string.IsNullOrEmpty(lparamName) ? null : new ApiParam() {
                                Type = NullifyEmpty(lparamType),
                                Name = NullifyEmpty(lparamName),
                                Default = NullifyEmpty(lparamDefault),
                            },
                        };
                        comment.Clear();
                        continue;
                    }
                }
                {
                    var match = evtRegex.Match(line);
                    if (match.Success)
                    {
                        string type = match.Groups["type"].Value;
                        string name = match.Groups["name"].Value;
                        string rawValue = match.Groups["value"].Value;
                        long? value = ParseValue(rawValue, out bool isHex);
                        string returnType = match.Groups["returnType"].Value;
                        string parameters = match.Groups["params"].Value;

                        yield return new ApiEvent() {
                            Type = type,
                            Name = name,
                            Category = category,
                            Comment = comment.ToString(),
                            RawValue = NullifyEmpty(rawValue),
                            Value = value,
                            IsHexValue = isHex,
                            ReturnType = returnType,
                            Parameters = parameters == "void" ? [] :
                                parameters.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(p => {
                                    string parameter = p.Trim();
                                    Match paramMatch = paramRegex.Match(parameter);
                                    string paramType = paramMatch.Groups["paramType"].Value;
                                    string paramName = paramMatch.Groups["paramName"].Value;
                                    string paramDefault = paramMatch.Groups["paramDefault"].Value;
                                    return new ApiParam() {
                                        Type = NullifyEmpty(paramType),
                                        Name = NullifyEmpty(paramName),
                                        Default = NullifyEmpty(paramDefault),
                                    };
                                }).ToArray(),
                        };
                        comment.Clear();
                        continue;
                    }
                }
            }
        }
    }

    private static readonly string[] functionTypes = ["fun", "get", "set"];

    public static IEnumerable<string> GenerateConstants(IEnumerable<ApiFeature> api)
    {
        bool first = true;
        ApiEnum[] lastGroup = [];
        foreach (ApiValue feature in api.OfType<ApiValue>())
        {
            if (feature.Value.HasValue)
            {
                {
                    if (!first && !(feature is ApiEnumValue apiEnumValue && apiEnumValue.Enums.SequenceEqual(lastGroup)))
                    {
                        yield return "";
                    }
                }

                {
                    if (feature is ApiEnumValue apiEnumValue && !apiEnumValue.Enums.SequenceEqual(lastGroup))
                    {
                        if (apiEnumValue.Enums.Length > 0)
                        {
                            int maxLen = 3;
                            foreach (string name in apiEnumValue.Enums.Select(x => x.Name))
                            {
                                yield return $"// {name}";
                                if (name.Length > maxLen)
                                    maxLen = name.Length;
                            }
                            if (maxLen > 0)
                                yield return $"// {new string('=', maxLen)}";
                        }
                    }
                }

                bool isFunction = functionTypes.Contains(feature.Type);
                bool isEvent = feature.Type == "evt";
                bool isFunctionLike = isFunction || isEvent;

                bool hasComment = !string.IsNullOrWhiteSpace(feature.Comment);

                if (hasComment || isFunctionLike)
                {
                    StringBuilder commentBuilder = new();
                    commentBuilder.Append(feature.Comment);
                    commentBuilder = commentBuilder.Replace("<", "&lt;");
                    commentBuilder = commentBuilder.Replace(">", "&gt;");

                    string comment = commentBuilder.ToString();
                    string[] commentLines = comment.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

                    if (isFunctionLike)
                    {
                        yield return "/// <summary>";
                        yield return $"/// <code>{feature}</code>";
                        if (hasComment)
                        {
                            foreach (string line in commentLines)
                            {
                                yield return $"/// {line}";
                            }
                        }
                        yield return "/// </summary>";
                    }
                    else if (hasComment)
                    {
                        if (commentLines.Length == 1)
                        {
                            yield return $"/// <summary>{comment}</summary>";
                        }
                        else if (commentLines.Length > 1)
                        {
                            yield return "/// <summary>";
                            foreach (string line in commentLines)
                            {
                                yield return $"/// {line}";
                            }
                            yield return "/// </summary>";
                        }
                    }
                }

                {
                    string prefix = "";
                    if (feature.Category == "Deprecated")
                        prefix = """[Obsolete("Deprecated")] """;
                    string name =
                        isFunction ? "SCI_" + feature.Name.ToUpperInvariant() :
                        isEvent ? "SCN_" + feature.Name.ToUpperInvariant() :
                        feature.Name;
                    yield return $"{prefix}public const {(feature.IsHexValue ? "uint" : "int")} {name} = {feature.RawValue};";
                }
            }
            lastGroup = (feature as ApiEnumValue)?.Enums ?? [];
            first = false;
        }
    }

    public static void GenerateConstants(IEnumerable<ApiFeature> api, TextWriter output, string indentation = "")
    {
        foreach (string text in GenerateConstants(api))
        {
            output.WriteLine("{0}{1}", indentation, text);
        }
    }
}

public class ApiFeature
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Comment { get; set; }
}

public class ApiValue : ApiFeature
{
    public string RawValue { get; set; }
    public long? Value { get; set; }
    public bool IsHexValue { get; set; }
}

public class ApiEnum : ApiFeature
{
    public string[] Prefixes { get; set; }
}

public class ApiEnumValue : ApiValue
{
    private ApiEnum[] enums;
    public ApiEnum[] Enums { get => enums ?? []; set => enums = value ?? []; }
}

public class ApiLexer : ApiEnum
{
    public string LexerName { get; set; }
}

public class ApiLexerValue : ApiEnumValue
{
    public ApiLexer[] Lexers { get => Enums as ApiLexer[]; set => Enums = value; }
}

public class ApiFunction : ApiValue
{
    public string ReturnType { get; set; }
    public ApiParam? Wparam { get; set; }
    public ApiParam? Lparam { get; set; }

    public override string ToString()
    {
        return $"{Type} {ReturnType} {Name}={RawValue}({string.Join(", ", Wparam?.ToString() ?? "", Lparam?.ToString() ?? "")})";
    }
}

public class ApiEvent : ApiValue
{
    public string ReturnType { get; set; }
    public ApiParam[] Parameters { get; set; }

    public override string ToString()
    {
        return $"{Type} {ReturnType} {Name}={RawValue}({string.Join(", ", Parameters)})";
    }
}

public struct ApiParam
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string Default { get; set; }

    public override readonly string ToString()
    {
        return $"{Type} {Name}{(string.IsNullOrEmpty(Default) ? "" : "=" + Default)}";
    }
}
