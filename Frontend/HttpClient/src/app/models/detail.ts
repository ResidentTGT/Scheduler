export class Detail {
    public id?: number|{};
    public title: string;
    public description: string;
    public cost?: number;
    public isPurchased?: Boolean = false;
    public routeId?: number;
}