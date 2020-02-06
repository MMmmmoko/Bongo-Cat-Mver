#include "header.hpp"
#include <resource.h>

#define VERSION "Bongo Cat Mver v0.1.2"



sf::RenderWindow window;

int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{

	window.create(sf::VideoMode(612, 352), VERSION, sf::Style::Titlebar | sf::Style::Close);

    // get refresh rate and set the frame limit
    DISPLAY_DEVICE device;
    ZeroMemory(&device, sizeof(device));
    device.cb = sizeof(device);
    EnumDisplayDevices(NULL, (DWORD)0, &device, 0);
    DEVMODE devmode;
    ZeroMemory(&devmode, sizeof(DEVMODE));
    devmode.dmSize = sizeof(DEVMODE);
    EnumDisplaySettings((LPSTR)device.DeviceName, ENUM_REGISTRY_SETTINGS, &devmode);

    // loading configs
    while (!data::init())
        continue;

    bool is_reload = false;

	window.setFramerateLimit(data::cfg["decoration"]["framerateLimit"].asInt());//���֡��
	sf::WindowHandle handle = window.getSystemHandle();
	HICON hIcon;
	hIcon = LoadIcon(hInstance, (LPCSTR)IDI_ICON1);
	if (hIcon) {
		SendMessage(handle, WM_SETICON, ICON_SMALL, (LPARAM)hIcon);
	}
	if (data::cfg["decoration"]["topWindow"].asBool())//�ö�����
	{
		SetWindowPos(handle, HWND_TOPMOST, 1, 1, 1, 1, SWP_NOMOVE | SWP_NOSIZE);
	}


  

    while (window.isOpen())
    {
        sf::Event event;
        while (window.pollEvent(event))
            switch (event.type)
            {
            case sf::Event::Closed:
                window.close();
                break;

            // get reload config prompt
            case sf::Event::KeyPressed:
                if (event.key.code == sf::Keyboard::R && event.key.control)
                {
                    if (!is_reload)
                        while (!data::init())
                            continue;
                    is_reload = true;
                    break;
                }
            default:
                is_reload = false;
            }

        int mode = data::cfg["mode"].asInt();
        int red_value = data::cfg["decoration"]["rgb"][0].asInt();
        int green_value = data::cfg["decoration"]["rgb"][1].asInt();
        int blue_value = data::cfg["decoration"]["rgb"][2].asInt();

        window.clear(sf::Color(red_value, green_value, blue_value));
        switch (mode)
        {
		case 1:
            standard::draw();
			break;
		case 2:
			keyboard::draw();
			break;
        }

        window.display();
    }

    return 0;
}
