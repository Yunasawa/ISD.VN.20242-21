namespace YNL.JAMOS
{
    public enum BookGenre : byte
    {
        Fiction, NonFiction, Mystery, Thriller, Fantasy, ScienceFiction, Romance, Horror, Historical, Biography,
        Memoir, Poetry, Drama, Adventure, Crime, Comedy, GraphicNovel, Children, YoungAdult, SelfHelp,
        Psychology, Philosophy, Business, Economics, Science, Technology, Education, Religion, Spirituality, Travel,
        Cookbooks, Art, Music, Health, Sports, Political, TrueCrime
    }

    public enum MusicGenre : byte
    {
        Pop, Rock, Alternative, Indie, Metal, Punk, HipHop, Rap, RnB, Soul,
        Jazz, Blues, Country, Folk, Classical, Opera, Electronic, House, Techno, Trance,
        Dubstep, Ambient, Reggae, Ska, Latin, KPop, JPop, Bollywood, Gospel, Worship,
        Soundtrack, Instrumental, Funk, Disco, NewWave, Experimental, LoFi, Choral, World, Traditional
    }

    public enum MovieGenre : byte
    {
        Action, Adventure, Comedy, Drama, Fantasy, Horror, Mystery, Thriller, ScienceFiction, Animation,
        Family, Musical, Documentary, Biography, Historical, Crime, War, Western, Superhero, Romance,
        Sports, Political, TrueCrime, Experimental, Indie, Noir, Silent, Mockumentary
    }

    public enum GameGenre : byte
    {
        Action, Adventure, RolePlaying, Shooter, Fighting, Survival, Horror, Platformer, Puzzle, Strategy,
        Simulation, Sports, Racing, OpenWorld, Sandbox, MMORPG, MOBA, TowerDefense, RealTimeStrategy, TurnBasedStrategy,
        CardGame, Roguelike, Metroidvania, VisualNovel, Rhythm, Party, Trivia, Tactical, Stealth, BattleRoyale,
        InteractiveStory, HackAndSlash, Soulslike, SurvivalHorror, Tycoon, LifeSimulation, Farming, DatingSim, Idle, Educational
    }
}