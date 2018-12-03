import { GroupBlock } from './GroupBlock';

export class OrderBlock {
    public id: number;

    public productionItemId: number;

    public productionItemsName: string;

    public productionItemsCount: number;

    public startTime: number;

    public duration: number;

    public isMachining: boolean;

    public groupBlocks: GroupBlock[];

    static fromJSON(obj: any) {
        if (!obj) {
            return null;
        }

        return Object.assign(
            new OrderBlock(),
            obj,
            {
                groupBlocks: obj.groupBlocks ? obj.groupBlocks.map(g => GroupBlock.fromJSON(g)) : []
            }
        );
    }
}
