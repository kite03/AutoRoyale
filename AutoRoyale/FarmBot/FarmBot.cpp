#include <iostream>
#include <Windows.h>

COLORREF getColAtPixel(POINT p)
{

    HDC hDC = GetDC(NULL);

    if (hDC == NULL)
    {
        return NULL;
    }

    COLORREF color = GetPixel(hDC, p.x, p.y);
    if (color == CLR_INVALID)
    {
        return NULL;
    }

    ReleaseDC(GetDesktopWindow(), hDC);

    return color;
}


void clickAtPos(POINT p)
{
    SetCursorPos(p.x, p.y);
    mouse_event(0x0002, p.x, p.y, 0, 0);
    mouse_event(0x0004, p.x, p.y, 0, 0);
}

int main()
{
    std::cout << "Fire Spirit Event Bot Farm 2022 Working Undetected!" << std::endl;

    
    /*
    while (true) {
        POINT p;
        if (GetCursorPos(&p))
        {
            std::cout << p.x << " " << p.y << "col:" << getColAtPixel(p) << std::endl;
         
        }
    }
    */

    //party button:
    // 2300 881col:16756736
    POINT pb;
        pb.x = 2300;
        pb.y = 881;
        COLORREF pbCol = 16756736;

    // challenge button:
    // 2314 974col:5294335
        POINT challenge;
        challenge.x = 2314;
        challenge.y = 974;
        COLORREF challengeCol = 5294335;

    // restart button:
    // 2089 1238col:16756558
        POINT pg;
        pg.x = 2089;
        pg.y = 1238;
        COLORREF restartCol = 16756558;

        // nomore rewarsdadfds:
    // 2136 888col:13222069
        POINT g;
        g.x = 2136;
        g.y = 888;
        COLORREF rewardCol = 13222069;

        srand(time(0));

        bool weed = false;
    while (true)
    {
        Sleep(1000);
        COLORREF col = getColAtPixel(pb);
        std::cout << col << std::endl;
        if (col == pbCol) {
            std::cout << "starting new game" << std::endl;
            clickAtPos(pb);

            Sleep(3000);

            col = getColAtPixel(challenge);
            std::cout << col << std::endl;
            if (col == challengeCol) {
                std::cout << "clcikcing chalaklgne" << std::endl;
                clickAtPos(challenge);
            }

            Sleep(3000);
            col = getColAtPixel(g);
            std::cout << col << std::endl;
            if (col == rewardCol) {
                std::cout << "no1 cares game" << std::endl;
                clickAtPos(g);
            }
        }

        Sleep(1000);

        col = getColAtPixel(pg);
        std::cout << col << std::endl;
        if (col == restartCol) {
            std::cout << "OOOOOOOOOOOK." << std::endl;
            clickAtPos(pg);
            Sleep(20000);
        }
        else
        {
            std::cout << "playig troop" << std::endl;
            // we are in game!!!
            int rNum = rand() % 4;
            rNum++; // to avoid 0;

            POINT troops;
            switch (rNum)
            {
            case 1:
                troops.x = 2000;
                troops.y = 1221;
                break;
            case 2:
                troops.x = 2104;
                troops.y = 1221;
                break;
            case 3:
                troops.x = 2289;
                troops.y = 1221;
                break;
            case 4:
                troops.x = 2427;
                troops.y = 1221;
                break;
            }

            clickAtPos(troops);
            Sleep(300);

            POINT p;
            if (weed)
            {
                p.x = 2116;
            }
            else
            {
                p.x = 2151;
            }
            p.y = 735;
            weed = !weed;
            clickAtPos(p);
        }
    }

}