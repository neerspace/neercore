using FluentValidation;
using NeerCore.Localization;

namespace NeerCoreTestingSuite.WebApp;

public static class LocalizedStringFluentValidationExtensions
{
    public static void HasAnyLocalization<T>(this IRuleBuilder<T, LocalizedString> builder) =>
        builder.Must(s => s.Any());

    public static void HasAnyLocalizationFrom<T>(this IRuleBuilder<T, LocalizedString> builder, params string[] languageCodes) =>
        builder.Must(s => languageCodes.Any(s.ContainsLocalization));

    public static void HasLocalization<T>(this IRuleBuilder<T, LocalizedString> builder, string languageCode) =>
        builder.Must(s => s.ContainsLocalization(languageCode));

    public static void HasLocalizationFrom<T>(this IRuleBuilder<T, LocalizedString> builder, params string[] languageCodes) =>
        builder.Must(s => languageCodes.All(s.ContainsLocalization));

    public static IRuleBuilderInitialCollection<LocalizedString, string> ForEachValue<T>(this IRuleBuilder<T, LocalizedString> builder)
    {
        IRuleBuilderInitialCollection<LocalizedString, string>? rule = null;
        builder.ChildRules(v => rule = v.RuleForEach(s => s.AsDictionary().Values));
        return rule!;
    }

    public static IRuleBuilderInitialCollection<LocalizedString, string> ForEachLanguageCode<T>(this IRuleBuilder<T, LocalizedString> builder)
    {
        IRuleBuilderInitialCollection<LocalizedString, string>? rule = null;
        builder.ChildRules(v => rule = v.RuleForEach(s => s.AsDictionary().Keys));
        return rule!;
    }
}