
export class Court{
  constructor(
    public id: string,
    public sport: string,
    public isHall: boolean,
    public name: string,
    public latitude: number,
    public longitude: number,
    public address: string,
    public startTime: number,
    public endTime: number,
    public price: number,
    public image?: string,
    public toShow?: boolean

  ) {
    this.toShow = true;
  }

}
