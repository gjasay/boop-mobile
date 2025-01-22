export class Queue
{
  public length: number = 0;
  public increment(): void
  {
    this.length++;
    console.log("Added to queue. Length: ", this.length);
  }

  public decrement(): void
  {
    this.length--;
    console.log("Removed from queue. Length: ", this.length);
  }

  /***
   * Await the queue to be empty
   * @returns A promise that resolves when the queue is empty
   ***/
  async queueStatus(): Promise<void>
  {
    return new Promise((resolve) => {
      const interval = setInterval(() => {
        if (this.length === 0) {
          console.log("Queue is empty.");
          clearInterval(interval);
          resolve();
        }
      }, 100);
    });
  }
}