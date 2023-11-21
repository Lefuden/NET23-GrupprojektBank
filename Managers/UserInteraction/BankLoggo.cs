using Spectre.Console;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal static class BankLoggo
    {
        private static readonly string HHHLoggo =
@"   
                   \ / \/ \/ / ,
                 \ /  \/ \/  \/  / ,
               \ \ \/ \/ \/ \ \/ \/ /
             .\  \/  \/ \/ \/  \/ / / /
            '  / / \/  \/ \/ \/  \/ \ \/ \
         .'     ) \/ \/ \/ \/  \/  \/ \ / \
        /   o    ) \/ \/ \/ \/ \/ \/ \// /
      o'_ ',__ .'   ,.,.,.,.,.,.,.,'- '%
";

        public static void LoginLoadingScreen()
        {
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Clear();
            int maxDuration = 100;
            int remainingDuration = 0;
            int i = 1;
            while (remainingDuration <= maxDuration)
            {
                AnsiConsole.Clear();
                // 
                WriteLogoAnimation(remainingDuration);
                int progress = (int)((double)remainingDuration / maxDuration * 100);
                AnsiConsole.MarkupLine($"[gold3_1]|{new string('#', progress / 2)}|[/][lightgoldenrod2]{new string('-', 50 - progress / 2)}| {progress}%[/]");
                remainingDuration++;
                Thread.Sleep(10);
            }
            while (i < 20)
            {
                AnsiConsole.Clear();
                if (i % 2 == 0)
                {
                    AnsiConsole.MarkupLine(BankLoggo.GetLoggo1());
                }
                else if (i % 3 == 0)
                {
                    AnsiConsole.MarkupLine(BankLoggo.GetLoggo2());
                }
                else
                {
                    AnsiConsole.MarkupLine(BankLoggo.GetLoggo3());
                }
                i++;
                Thread.Sleep(150);
            }

            AnsiConsole.Cursor.Show();
        }
        private static void WriteLogoAnimation(int currentProgress)
        {
            AnsiConsole.MarkupLine(UpdateBankLoggoAnimation(currentProgress));
        }

        private static string UpdateBankLoggoAnimation(int currentProgress)
        {
            AnsiConsole.Cursor.SetPosition(0, 0);
            return currentProgress switch
            {

                >= 90 =>
               HHHLoggo +
               @"              //||           // \\ 
             '' ''          ''  ''
              [lightgoldenrod2]H[/][gold1]yper Hedgehogs Fundings[/]",

                >= 80 =>
                HHHLoggo +
                @"             // //          //|| 
            '' ''          '' ''
              [gold1]Hyp[/][lightgoldenrod2]e[/][gold1]r Hedgehogs Fundings[/]",

                >= 70 =>
                 HHHLoggo +
                 @"              //||           //\\ 
             '' ''          '' ''
              [gold1]Hyper H[/][lightgoldenrod2]e[/][gold1]dgehogs Fundings[/]",

                >= 60 =>
                HHHLoggo +
                @"               //\\           //\\ 
              '' ''         ''  ''
              [gold1]Hyper Hed[/][lightgoldenrod2]g[/][gold1]ehogs Fundings[/]",
                >= 50 =>
                HHHLoggo +
                @"               // \\          // \\ 
              ''  ''         ''  ''
              [gold1]Hyper Hedge[/][lightgoldenrod2]h[/][gold1]ogs Fundings[/]",
                >= 40 =>

                HHHLoggo +
                @"               //\\           //\\ 
              '' ''         ''  ''
              [gold1]Hyper Hedgeh[/][lightgoldenrod2]o[/][gold1]gs Fundings[/]",
                >= 30 =>

                HHHLoggo +
                @"              //||           //\\ 
             '' ''          '' ''
              [gold1]Hyper Hedgehogs [/][lightgoldenrod2]F[/][gold1]undings[/]",
                >= 20 =>

                HHHLoggo +
                @"             // //          //|| 
            '' ''          '' ''
              [gold1]Hyper Hedgehogs Fu[/][lightgoldenrod2]n[/][gold1]dings[/]",
                >= 10 =>

                HHHLoggo +
                @"              //||           // \\ 
             '' ''          ''  ''
              [gold1]Hyper Hedgehogs Fund[/][lightgoldenrod2]i[/][gold1]ngs[/]",

                <= 10 =>
               HHHLoggo +
               @"                // \\          // \\ 
                ''  ''         ''  ''
              [gold1]Hyper Hedgehogs Fundin[/][lightgoldenrod2]g[/][gold1]s[/]",
            };
        }
        public static string GetLoggo1() =>
@"                                                                                          [gold3_1]
      __  __                                                                   
     /\ \/\ \                                                                  
     \ \ \_\ \  __  __  _____      __   _ __                                                [/][gold1]                          
      \ \  _  \/\ \/\ \/\ '__`\  /'__`\/\`'__\                                                                        
       \ \ \ \ \ \ \_\ \ \ \L\ \/\  __/\ \ \/                                               [/][lightgoldenrod1]                          
        \ \_\ \_\/`____ \ \ ,__/\ \____\\ \_\                                               [/][gold1]                         
         \/_/\/_/`/___/> \ \ \/  \/____/ \/_/                                  
                    /\___/\ \_\                                                             [/][gold3_1]                          
      __  __        \/__/__\/_/              __                                             [/][lightgoldenrod1]                   
     /\ \/\ \           /\ \                /\ \                               
     \ \ \_\ \     __   \_\ \     __      __\ \ \___     ___      __     ____               [/][gold1]  
      \ \  _  \  /'__`\ /'_` \  /'_ `\  /'__`\ \  _ `\  / __`\  /'_ `\  /',__\ 
       \ \ \ \ \/\  __//\ \L\ \/\ \L\ \/\  __/\ \ \ \ \/\ \L\ \/\ \L\ \/\__, `\
        \ \_\ \_\ \____\ \___,_\ \____ \ \____\\ \_\ \_\ \____/\ \____ \/\____/             [/][gold3_1]  
         \/_/\/_/\/____/\/__,_ /\/___L\ \/____/ \/_/\/_/\/___/  \/___L\ \/___/              [/][gold1]  
                                  /\____/                         /\____/                   
      ____                      __\_/__/                          \_/__/                    [/][gold3_1]     
     /\  _`\                   /\ \  __                                                     [/][lightgoldenrod1]   
     \ \ \L\_\__  __    ___    \_\ \/\_\    ___      __     ____                            [/][gold1]  
      \ \  _\/\ \/\ \ /' _ `\  /'_` \/\ \ /' _ `\  /'_ `\  /',__\                           
       \ \ \/\ \ \_\ \/\ \/\ \/\ \L\ \ \ \/\ \/\ \/\ \L\ \/\__, `\                          [/][gold3_1]   
        \ \_\ \ \____/\ \_\ \_\ \___,_\ \_\ \_\ \_\ \____ \/\____/                          
         \/_/  \/___/  \/_/\/_/\/__,_ /\/_/\/_/\/_/\/___L\ \/___/                           
                                                     /\____/                                [/][gold1] 
                                                     \_/__/                                 [/]";
        public static string GetLoggo2() =>

@"                                                                                          [gold3_1]
      __  __                                                                   
     /\ \/\ \                                                                  
     \ \ \_\ \  __  __  _____      __   _ __                                                [/][gold1]                          
      \ \  _  \/\ \/\ \/\ '__`\  /'__`\/\`'__\                                                                        
       \ \ \ \ \ \ \_\ \ \ \L\ \/\  __/\ \ \/                                                                         
        \ \_\ \_\/`____ \ \ ,__/\ \____\\ \_\                                               [/][gold1]                         
         \/_/\/_/`/___/> \ \ \/  \/____/ \/_/                                               [/][lightgoldenrod1]
                    /\___/\ \_\                                                             [/][gold3_1]                          
      __  __        \/__/__\/_/              __                                                                
     /\ \/\ \           /\ \                /\ \                               
     \ \ \_\ \     __   \_\ \     __      __\ \ \___     ___      __     ____               [/][gold1]  
      \ \  _  \  /'__`\ /'_` \  /'_ `\  /'__`\ \  _ `\  / __`\  /'_ `\  /',__\              [/][lightgoldenrod1]
       \ \ \ \ \/\  __//\ \L\ \/\ \L\ \/\  __/\ \ \ \ \/\ \L\ \/\ \L\ \/\__, `\
        \ \_\ \_\ \____\ \___,_\ \____ \ \____\\ \_\ \_\ \____/\ \____ \/\____/             [/][gold3_1]  
         \/_/\/_/\/____/\/__,_ /\/___L\ \/____/ \/_/\/_/\/___/  \/___L\ \/___/              [/][gold1]  
                                  /\____/                         /\____/                   
      ____                      __\_/__/                          \_/__/                    [/][gold3_1]     
     /\  _`\                   /\ \  __                                                        
     \ \ \L\_\__  __    ___    \_\ \/\_\    ___      __     ____                            [/][gold1]  
      \ \  _\/\ \/\ \ /' _ `\  /'_` \/\ \ /' _ `\  /'_ `\  /',__\                           
       \ \ \/\ \ \_\ \/\ \/\ \/\ \L\ \ \ \/\ \/\ \/\ \L\ \/\__, `\                          [/][gold3_1]   
        \ \_\ \ \____/\ \_\ \_\ \___,_\ \_\ \_\ \_\ \____ \/\____/                          [/][lightgoldenrod1]
         \/_/  \/___/  \/_/\/_/\/__,_ /\/_/\/_/\/_/\/___L\ \/___/                           
                                                     /\____/                                [/][gold1] 
                                                     \_/__/                                 [/]";

        public static string GetLoggo3() =>

@"                                                                                          [gold3_1]
      __  __                                                                   
     /\ \/\ \                                                                  
     \ \ \_\ \  __  __  _____      __   _ __                                                [/][gold1]                          
      \ \  _  \/\ \/\ \/\ '__`\  /'__`\/\`'__\                                                                        
       \ \ \ \ \ \ \_\ \ \ \L\ \/\  __/\ \ \/                                                                         
        \ \_\ \_\/`____ \ \ ,__/\ \____\\ \_\                                               [/][gold1]                         
         \/_/\/_/`/___/> \ \ \/  \/____/ \/_/                                               
                    /\___/\ \_\                                                             [/][gold3_1]                          
      __  __        \/__/__\/_/              __                                                                
     /\ \/\ \           /\ \                /\ \                                            [/][lightgoldenrod1]
     \ \ \_\ \     __   \_\ \     __      __\ \ \___     ___      __     ____               [/][gold1]  
      \ \  _  \  /'__`\ /'_` \  /'_ `\  /'__`\ \  _ `\  / __`\  /'_ `\  /',__\              
       \ \ \ \ \/\  __//\ \L\ \/\ \L\ \/\  __/\ \ \ \ \/\ \L\ \/\ \L\ \/\__, `\
        \ \_\ \_\ \____\ \___,_\ \____ \ \____\\ \_\ \_\ \____/\ \____ \/\____/             [/][gold3_1]  
         \/_/\/_/\/____/\/__,_ /\/___L\ \/____/ \/_/\/_/\/___/  \/___L\ \/___/              [/][lightgoldenrod1]
                                  /\____/                         /\____/                   
      ____                      __\_/__/                          \_/__/                    [/][gold3_1]     
     /\  _`\                   /\ \  __                                                        [/][gold1]  
     \ \ \L\_\__  __    ___    \_\ \/\_\    ___      __     ____                            [/][gold1]  
      \ \  _\/\ \/\ \ /' _ `\  /'_` \/\ \ /' _ `\  /'_ `\  /',__\                           
       \ \ \/\ \ \_\ \/\ \/\ \/\ \L\ \ \ \/\ \/\ \/\ \L\ \/\__, `\                          [/][gold3_1]   
        \ \_\ \ \____/\ \_\ \_\ \___,_\ \_\ \_\ \_\ \____ \/\____/                          
         \/_/  \/___/  \/_/\/_/\/__,_ /\/_/\/_/\/_/\/___L\ \/___/                           [/][gold1] 
                                                     /\____/                                
                                                     \_/__/                                 [/][lightgoldenrod1]
[/]";
        public static string SAAANIC() =>
    @"
                   \ / [wheat4]\/ [/] [grey53]\/ [/] [rosybrown] /[/] [grey69],
                 \ [/]/  [navajowhite1]\/[/] [wheat4]\/ [/]  [grey53]/ [/][rosybrown]  [/][grey69]\/ [/]  [navajowhite1]/ [/][grey69],[/]
               \ \ [rosybrown]\/ [/] [wheat4]\/ [/] [grey69] \ [/][navajowhite1]\/ [/] [grey69] \ [/][wheat4]\/ [/] [rosybrown] / [/][grey69] /[/]
             .\ [rosybrown] \/ [/] [wheat4]\/ [/] [grey69] \/ [/] [rosybrown] \/ [/] [grey53] \/ [/] [navajowhite1]\/ [/][wheat4] \/ [/][grey69] / [/][grey53] /
            '  [/][navajowhite1]/ [/][wheat4] \/ [/] [grey69] \/ [/] [rosybrown] \/ [/] [grey69] \ [/][grey53] \/ [/] [navajowhite1] \ [/][grey53] \/ [/] [wheat4] /[/]
         .'     ) [wheat4] \/ [/] [grey69] \/ [/] [rosybrown] \/ [/] [grey69]  \/ [/] [grey53] \/ [/] [navajowhite1] \[/] [grey69] \ [/][wheat4] \/[/] [grey69] /[/]
        /   o    ) [rosybrown] \/ [/] [wheat4] \/ [/] [grey69] \/ [/] [rosybrown] \/ [/] [grey69] \ [/][navajowhite1] \/[/] [grey53] /
      o'_ ',__ .'  [/] [wheat4],[/] [grey53],[/] [rosybrown], [/][grey53],[/] [rosybrown],'- ' [/][grey69]%[/]
               // \\          // \\ 
              ''  ''         ''  ''
";


        public static string HHHHHHHHHHHHH() =>
            @"
[lightgoldenrod2]H[/][gold1]yper Hedgehogs Fundings
H[/][lightgoldenrod2]y[/][gold1]per Hedgehogs Fundings
Hyp[/][lightgoldenrod2]e[/][gold1]r Hedgehogs Fundings
Hype[/][lightgoldenrod2]r[/][gold1] Hedgehogs Fundings
Hyper[/][lightgoldenrod2] [/][gold1]Hedgehogs Fundings
Hyper H[/][lightgoldenrod2]e[/][gold1]dgehogs Fundings
Hyper He[/][lightgoldenrod2]d[/][gold1]gehogs Fundings
Hyper Hed[/][lightgoldenrod2]g[/][gold1]ehogs Fundings
Hyper Hedg[/][lightgoldenrod2]e[/][gold1]hogs Fundings
Hyper Hedge[/][lightgoldenrod2]h[/][gold1]ogs Fundings
Hyper Hedgeh[/][lightgoldenrod2]o[/][gold1]gs Fundings
Hyper Hedgeho[/][lightgoldenrod2]g[/][gold1]s Fundings
Hyper Hedgehog[/][lightgoldenrod2]s[/][gold1] Fundings
Hyper Hedgehogs [/][lightgoldenrod2]F[/][gold1]undings
Hyper Hedgehogs F[/][lightgoldenrod2]u[/][gold1]ndings
Hyper Hedgehogs Fu[/][lightgoldenrod2]n[/][gold1]dings
Hyper Hedgehogs Fun[/][lightgoldenrod2]d[/][gold1]ings
Hyper Hedgehogs Fund[/][lightgoldenrod2]i[/][gold1]ngs
Hyper Hedgehogs Fundi[/][lightgoldenrod2]n[/][gold1]gs
Hyper Hedgehogs Fundin[/][lightgoldenrod2]g[/][gold1]s
Hyper Hedgehogs Funding[/][lightgoldenrod2]s[/]
";
    }
}