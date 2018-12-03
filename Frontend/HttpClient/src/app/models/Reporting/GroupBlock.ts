import { DetailsBatchBlock } from './DetailsBatchBlock';
import { Workshop } from '../workshop';
import { TransportOperationBlock } from './transport-operation-block';

export class GroupBlock {
    public id: number;

    public groupIndex: number;

    public workshop: Workshop;

    public startTime: number;

    public duration: number;

    public transportStartTime: number;

    public transportDuration: number;

    public detailsBatchBlocks: DetailsBatchBlock[];

    public transportOperationBlock: TransportOperationBlock;

    static fromJSON(obj: any) {
        if (!obj) {
            return null;
        }

        return Object.assign(
            new GroupBlock(),
            obj,
            {
                detailsBatchBlocks: obj.detailsBatchBlocks ? obj.detailsBatchBlocks.map(g => DetailsBatchBlock.fromJSON(g)) : [],
                transportOperationBlock: obj.transportOperationBlock ? TransportOperationBlock.fromJSON(obj.transportOperationBlock) : null
            }
        );
    }
}
