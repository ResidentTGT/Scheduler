import { Equipment } from '../equipment';

export class DetailsBatchBlock {
    public id: number;

    public detailId: number;

    public detailName: string;

    public detailsCount: number;

    public startTime: number;

    public duration: number;

    public equipment: Equipment;

    static fromJSON(obj: any) {
        if (!obj) {
            return null;
        }

        return Object.assign(
            new DetailsBatchBlock(),
            obj,
            {
                equipment: obj.equipment ? Equipment.fromJSON(obj.equipment) : null
            }
        );
    }
}
