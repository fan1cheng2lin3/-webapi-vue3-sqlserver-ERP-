<template>
  <div class="app">
    <h2>句子发音</h2>
    <textarea 
      v-model="text" 
      placeholder="输入单词或句子..."
      rows="4" 
      cols="50">
    </textarea>
    
    <div class="controls">
      <label>
        <input type="radio" value="en-US" v-model="selectedVoice" />
        美音
      </label>
      <label>
        <input type="radio" value="en-GB" v-model="selectedVoice" />
        英音
      </label>
      
      <div class="slider">
        <label for="rate">语速: {{ rate }}</label>
        <input 
          id="rate" 
          type="range" 
          min="0.5" 
          max="2" 
          step="0.1" 
          v-model="rate" />
      </div>
    </div>

    <button @click="speak">播放</button>
    <button @click="stop">停止</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      text: "", // 用户输入的句子或单词
      selectedVoice: "en-US", // 默认选择美音
      rate: 1, // 语速，初始为1（正常速度）
    };
  },
  methods: {
    speak() {
      if (!this.text.trim()) {
        alert("请输入内容后再播放！");
        return;
      }
      
      // 停止当前发音（防止重叠）
      speechSynthesis.cancel();

      // 创建 SpeechSynthesisUtterance 实例
      const utterance = new SpeechSynthesisUtterance(this.text);
      utterance.lang = this.selectedVoice; // 设置语言
      utterance.rate = this.rate; // 设置语速
      
      // 发音
      speechSynthesis.speak(utterance);
    },
    stop() {
      // 停止发音
      speechSynthesis.cancel();
    },
  },
};
</script>

<style>
.app {
  font-family: Arial, sans-serif;
  margin: 20px;
}

textarea {
  width: 100%;
  margin-bottom: 10px;
  font-size: 16px;
  padding: 5px;
}

.controls {
  margin: 10px 0;
  display: flex;
  align-items: center;
  gap: 20px;
}

.slider {
  display: flex;
  flex-direction: column;
  align-items: center;
}

button {
  margin: 5px;
  padding: 8px 16px;
  font-size: 16px;
  cursor: pointer;
}
</style>
