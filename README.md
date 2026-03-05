-Opsæting af projekt.

For at sikre, at hvert projekt kan køres både individuelt og samlet, kan hver gruppe oprette deres eget projekt og tilføje det til .sln-filen. 
På den måde kan alle køre deres kode separat, samtidig med at det er muligt at referere til hinandens projekter.

How to

1. Opret en mappe til jeres projekt, f.eks. PIM og PIM.Test.

2. I denne mappe placerer I alle filer, der hører til projektet. (Dette gælder også program.cs med Main methoden og avalonias auto generate filer). I kan eventuelt omdøbe eller oprette en ny .csproj-fil med samme navn som mappen (f.eks. \PIM\PIM.csproj).

3. Nu skal .csproj-filen tilføjes til .sln-filen, så projektet kan køres.
Åbn terminalen og naviger til den mappe, hvor .sln-filen ligger. (Hvis I kører ls, burde den dukke op).
Kør følgende kommando:

    ```xml
    dotnet sln add .\jeresMappeNavn\jeresCsprojNavn.csproj

4. Herefter burde projektet bede om en genindlæsning. Her efter er det bare vigtigt at i vælger at kører jeres eget projekt 


-Referere til hinandens projekter

For at tilgå en andens gruppes projekt, eksempelvis PIM. Skal man i sin egen .csproj-fil tilføje 

    ```xml 
    <ItemGroup>  
        <ProjectReference Include="..\PIM\PIM.csproj" />  
    </ItemGroup>
Herfra er det bare at slå sig løs.

Alle eventulle indvendinger går til Abishiyam – 
eller på Discord.

