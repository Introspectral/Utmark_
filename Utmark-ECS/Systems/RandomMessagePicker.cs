public class RandomMessagePicker
{
    private Random _random;
    private List<string> _messages;

    public RandomMessagePicker()
    {
        _random = new Random();

        _messages = new List<string>
        {
            "[color=red]*[/color] You see nothing to pick up.",
            "[color=red]*[/color] There's nothing here to grab.",
            "[color=red]*[/color] Your hands find nothing.",
            "[color=red]*[/color] Nothing worth picking up here.",
            "[color=red]*[/color] The ground is empty; nothing to collect.",
            "[color=red]*[/color] Seems like there's nothing for you to take.",
            "[color=red]*[/color] It's barren; not a thing to gather.",
            "[color=red]*[/color] You look around but find nothing to pick up.",
            "[color=red]*[/color] No items catch your eye.",
            "[color=red]*[/color] The area is clear; no objects to retrieve.",
            "[color=red]*[/color] You search, but there's nothing to take.",
            "[color=red]*[/color] It's all clear; nothing to hold onto.",
            "[color=red]*[/color] You can't find a single thing to collect.",
            "[color=red]*[/color] Your search yields no items to grab.",
            "[color=red]*[/color] Not a single item in sight to pick up.",
            "[color=red]*[/color] There's a lack of things to gather here.",
            "[color=red]*[/color] Your hands remain empty; nothing to get.",
            "[color=red]*[/color] You don't spot any items to collect.",
            "[color=red]*[/color] The vicinity has no objects for you to take.",
            "[color=red]*[/color] Seems empty; no items to retrieve."
        };
    }

    public string GetRandomMessage()
    {
        if (_messages.Count == 0)
        {
            throw new InvalidOperationException("No messages available to choose from.");
        }

        int randomIndex = _random.Next(_messages.Count);
        return _messages[randomIndex];
    }
}


