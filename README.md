# Senior Project
A repository housing code for the senior project done at Eastern Washington University

# Instructions for getting the client up and running

1. Install node/npm [here](https://nodejs.org/en/download/)
2. Run this command to install angular cli: `npm install -g @angular/cli`
3. cd into `book-order-app` 
4. Use the command: `ng serve --proxy-conf proxy.conf.json --host 0.0.0.0`

----------

*NOTE: When running locally, it may be necessary to run `npm install` after step 3*

# Instructions for getting the server up and running

Windows Route:

**See below** *(ensure that dotnet cli is installed so the system recognizes the commands)*

*nix Route: *(see [here](https://www.microsoft.com/net/core#linuxubuntu) for more details)*

1. cd to the bookorder.api folder (which houses the `.sln` file)
2. dotnet restore
3. dotnet build
4. cd src/bookorder.api
5. dotnet run

----------
*NOTE: This hasn't been fully tested yet so stay tuned for updates*

# Tips

Check out the github documentation for [Angular CLI](https://github.com/angular/angular-cli/wiki) for more information on how to easily generate new components, etc.

Suggested style guide information can be found [here](https://angular.io/guide/styleguide).

----------

*NOTE: Regularly run `ng test` often and before any pull requests to ensure existing functionality is not compromised.*