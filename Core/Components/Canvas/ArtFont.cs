using System.Collections.Immutable;

public class ArtFont : Font
{
    private readonly char[] _chars = ['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                                            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                                            '0','1','2','3','4','5','6','7','8','9',
                                            '.',',',';',':','?','!','-','#','"','_','&','(',')',
                                            '[',']','`','\\','/',' ','Б','В','+','=','*','$','<','>','%'];

    public ArtFont(Sequence sequence)
    {
        Sequence = sequence;
        ImmutableDictionary<char, int>.Builder builder = ImmutableDictionary.CreateBuilder<char, int>();

        for (int i = 0; i < _chars.Length; i++)
            builder.Add(_chars[i], i);

        Characters = builder.ToImmutableDictionary();
    }
    public override Sprite[] Create(string word)
    {
        char[] textChars = word.ToCharArray();
        int[] indexes = new int[textChars.Length];
        Sprite[] sprites = new Sprite[textChars.Length];

        for (int i = 0; i < textChars.Length; i++)
        {
            indexes[i] = Characters[textChars[i]];
            sprites[i] = Sequence[indexes[i]];
        }

        return sprites;
    }
}