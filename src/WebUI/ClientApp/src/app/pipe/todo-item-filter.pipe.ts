import { Pipe, PipeTransform } from "@angular/core";
import { TodoItemDto } from "../web-api-client";


@Pipe({
    name:'todoItemFilter',
    pure: false
})
export class TodoItemFilter implements PipeTransform {
    transform(items: TodoItemDto[], selectedTags: string[]) {
        if (!selectedTags ||
            selectedTags.length === 0)
            return items;

        return items.filter((item) => {
            return item.tags.findIndex(tag => selectedTags.includes(tag.name)) > -1;
        });
    }
}