export class TransportOperationBlock {
    public id: number;

    public firstWorkshopId: number;

    public firstWorkshopName: string;

    public secondWorkshopId: number;

    public secondWorkshopName: string;

    public distance: number;

    public duration: number;

    static fromJSON(obj: any) {
        if (!obj) {
            return null;
        }

        return Object.assign(
            new TransportOperationBlock(),
            obj
        );
    }
}
