import { IPlatformUser } from './IPlatformUser';

export class LoginResult {
    public User: IPlatformUser;
    public MessageCode: string;
    public Succeeded: boolean;
    public Token: string;
    public Roles: string[];
}
