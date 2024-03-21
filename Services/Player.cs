namespace dxt.Services;

public static class Player
{
    static long count = 0;
    readonly static List<Models.Player> container = [];

    public static List<Models.Player> GetAll() => container;

    public static void Add(Models.Player player) {
        player.Id = ++count;
        container.Add(player);
    }

    public static Models.Player? Get(long playerId) =>
        container.FirstOrDefault(item => item.Id == playerId);

    public static void Delete(long playerId) {
        if(Get(playerId) is Models.Player player)
            container.Remove(player);
    }

    public static void Update(Models.Player player) {
        var index = container.FindIndex(item => item.Id == player.Id);
        if(index != -1) container[index] = player;
    }
}
