export class User {
  constructor(public username: string,
              public id: string,
              public role: string,
              public image: string,
              public displayName: string,
              public address: string,
              private _token: string,
              private _tokenExpirationDate: Date) { }

  get token(){
    if(!this._tokenExpirationDate || new Date() > this._tokenExpirationDate){
      return null;
    }
    return this._token;
  }
}
