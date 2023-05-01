/*using Python.Runtime;
using System;

public class GptPythonConnection {
	string badPrompt = "Ayy, listen up. Here's the dealio, I need you to write me a(ONLY ONE) short informal DM with some bad news that's gonna make me wanna throw my phone out the window. Think like, \"yo dude, you ain't gonna believe this shit, but the party got busted by the cops and they took all our booze. Or maybe, \"bro, I just found out my ex is hooking up with my best friend, what the actual fuck?\" Message must be in 1-2 sentence, not more. Also message must be not about car towed, concert canceled or bar/pizza closed.";
	string goodPrompt = "Yo, my main man! " +
			"I need you to do me a favor again, but this time it's gonna be lit! " +
			"I need you to write me a DM with some amazing news that's gonna make me jump outta my seat. " +
			"Like, \"dude, you won the lottery and now you're a millionaire!\" Or maybe, \"bro, you just got accepted into your dream college, congrats!\" " +
			"Can you hook me up with some awesome news that's gonna make my day? Message must be in 1-2 sentence, not more. Let's do this!";

	string GetResponse(string prompt) {
		Initialize();
		using (Py.GIL()) {
			dynamic gpt4free = Py.Import("gpt4free");

			// it's forefront and this shit is doesn't works by some reason so if u want to u can delete it
			try {
				dynamic token = gpt4free.Provider.ForeFront.Account.create(logging: false);
				return gpt4free.Completion.create(gpt4free.Provider.ForeFront, prompt: prompt, model: "gpt-4", temperature: 0.82, frequency_penalty: 0.15, token: token);
			}
			catch {
				// it's Theb                                            request(good||bad) more=more random  punch gpt if he will repead
				return gpt4free.Completion.create(gpt4free.Provider.Theb, prompt: prompt, temperature: 0.82, frequency_penalty: 0.15);
			}
		}

		void Initialize() {   // python 3.6-3.11 with request libralies must be instaled
							  // pip install websocket-client requests tls-client pypasser names colorama curl_cffi streamlit==1.21.0 selenium fake-useragent twocaptcha pydantic pymailtm git+https://github.com/AI-Yash/st-chat/archive/refs/pull/24/head.zip
							  // this directory{https://github.com/xtekky/gpt4free/tree/main/gpt4free} have to be in python site directory{path by default: C:\Users\urusername\AppData\Local\Programs\Python\Python[ur_python_version]\Lib} or installed thru: pip install gpt4free (but it's didn't worked for me)
							  //username👇                                      ver       ver
			//Set this dll to a static one packaged in the app
			string pythonDll = @"C:\Users\raind\AppData\Local\Programs\Python\Python310\python310.dll";
			Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);
			PythonEngine.Initialize();
		}
	}
}*/