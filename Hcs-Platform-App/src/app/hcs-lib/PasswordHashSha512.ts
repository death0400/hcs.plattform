import { SHA512 } from 'crypto-js';
import { PasswordHash } from './PasswordHash';

export class PasswordHashSha512 extends PasswordHash {
    public hash(value: string): string {
        return SHA512(value).toString();
    }
}
