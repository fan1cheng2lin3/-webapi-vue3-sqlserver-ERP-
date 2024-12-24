<script setup>
import { reactive, ref, onMounted, watch } from 'vue';
import { ElMessage } from 'element-plus';
import axios from 'axios';
import { useRouter } from 'vue-router';
const router = useRouter();

// 自动生成订单编号
const generateOrderId = () => {
  const now = new Date();
  return (
    now.getMinutes().toString().padStart(2, '0') +
    now.getSeconds().toString().padStart(2, '0') +
    now.getMilliseconds().toString().padStart(3, '0')
  );
};

// 初始化产品信息
const initProduct = () => ({
  name: '',
  productCode: '',
  productType: '',
  supplierName: '',
  unitPrice: 0,
  count: 0,
});

const productTableData = reactive([initProduct()]);

// 表单引用
const purchaseFormRef = ref(null);

// 校验规则
const rules = reactive({
  contact_name: [{ required: true, message: '联系人不能为空', trigger: 'blur' }],
  contact_phone: [{ required: true, message: '联系电话不能为空', trigger: 'blur' }],
  unit_name: [{ required: true, message: '单位名称不能为空', trigger: 'blur' }],
  address: [{ required: true, message: '单位地址不能为空', trigger: 'blur' }],
  order_status: [{ required: true, message: '订单状态不能为空', trigger: 'change' }],
  created_by: [{ required: true, message: '创建人不能为空', trigger: 'blur' }],
});

// 添加新产品行
const addNewProduct = () => {
  productTableData.push(initProduct());
};

// 删除产品行
const deleteProduct = (index) => {
  if (productTableData.length > 1) { // 至少保留一个商品
    productTableData.splice(index, 1);
  } else {
    ElMessage.error("至少需要一个商品");
  }
};

// 表单数据：采购订单信息
const purchaseForm = reactive({
  sales_order_id: '', // 自动生成的订单编号
  contact_name: '',
  contact_phone: '',
  unit_name: '',
  address: '',
  order_status: '',
  created_by: '',
});

// 发货通知是否可见
const showShippingNotice = ref(false);

// 提交表单
const submitForm = async () => {
  // 先进行基本的表单校验
  purchaseFormRef.value.validate(async (valid) => {
    if (!valid) {
      ElMessage.error("请填写完整的个人信息");
      return; // 如果个人信息没有填写完整，阻止提交
    }

    // 检查至少有一个产品
    if (productTableData.length === 0 || !productTableData.some(product => product.name && product.productCode)) {
      ElMessage.error("请至少添加一个产品");
      return;
    }

    // 获取当前时间
    const currentDate = new Date().toISOString(); // 使用 ISO 8601 格式的日期时间字符串

    // 构建 payload，包括商品信息
    const payload = {
      sales_order_id: parseInt(purchaseForm.sales_order_id), // 确保销售订单ID是整数
      contact_name: purchaseForm.contact_name,
      contact_phone: purchaseForm.contact_phone,
      unit_name: purchaseForm.unit_name,
      address: purchaseForm.address,
      order_status: purchaseForm.order_status,
      created_by: purchaseForm.created_by,
      created_at: currentDate, // 当前时间
      order_date: currentDate, // 订单日期
      order_items: productTableData.map(product => ({
        sales_order_id: parseInt(purchaseForm.sales_order_id), // 确保销售订单ID在每个产品中也是整数
        name: product.name,
        product_code: product.productCode,
        product_type: product.productType, // 需要在表格中添加 productType 字段
        count: product.count,
        unit_price: product.unitPrice
      }))
    };

    try {
      await axios.post('/Sale/AddSales', payload);
      ElMessage.success('添加成功');
      
      // 审核完成，显示发货通知按钮
      showShippingNotice.value = true;
    } catch (error) {
      const message = error.response?.data?.message || '网络或服务端错误';
      ElMessage.error(`添加失败：${message}`);
    }
  });
};

// 重置表单
const resetForm = () => {
  if (purchaseFormRef.value) {
    purchaseFormRef.value.resetFields();
  }
  purchaseForm.sales_order_id = generateOrderId(); // 调用 generateOrderId() 来生成新的订单编号
  productTableData.splice(0);
  addNewProduct(); // 添加初始产品行
};

// 产品列表，用于后端查询
const productList = ref([]);

// 查询商品信息
const getProductList = async () => {
  try {
    const response = await axios.get('/File_Management/product');
    productList.value = response.data;  // 将商品数据存储在响应式列表中
  } catch (error) {
    ElMessage.error("无法加载商品数据");
  }
};

// 自动填充商品信息
const autoFillProduct = (index, productCodeOrName) => {
  const product = productList.value.find(
    (p) => p.product_code === productCodeOrName || p.name === productCodeOrName
  );
  if (product) {
    // 找到匹配的产品，填充信息
    productTableData[index].productCode = product.product_code;
    productTableData[index].name = product.name;
    productTableData[index].productType = product.product_type;
    productTableData[index].unitPrice = product.unit_price;
  } else {
   console.log('未找到匹配的产品');
  }
};

// 监听产品编码或产品名称的变化
watch(
  () => productTableData,
  (newData) => {
    newData.forEach((row, index) => {
      if (row.productCode || row.name) {
        autoFillProduct(index, row.productCode || row.name);
      }
    });
  },
  { deep: true }
);

const goToWarehouse = () => {
  router.push({ name: 'WarehouO' });

};

// 初始化订单编号
onMounted(() => {
  getProductList();
  purchaseForm.sales_order_id = generateOrderId();
});

</script>

<template>
    <h1>销售表</h1>
    <!-- 提交按钮 -->
    <el-form-item>
      <el-button type="primary" @click="submitForm">审核</el-button>
      <el-button @click="resetForm">重置</el-button>
      <!-- 只有审核完成后才显示发货通知按钮 -->
      <el-button v-if="showShippingNotice" @click="goToWarehouse">发货通知</el-button>
    </el-form-item>
  
  <el-form ref="purchaseFormRef" :model="purchaseForm" :rules="rules" label-width="120px">
    <!-- 采购订单信息 -->
    <el-form-item label="销售编号">
      <el-input v-model="purchaseForm.sales_order_id" disabled></el-input>
    </el-form-item>
    <el-form-item label="联系人">
      <el-input v-model="purchaseForm.contact_name"></el-input>
    </el-form-item>
    <el-form-item label="联系电话">
      <el-input v-model="purchaseForm.contact_phone"></el-input>
    </el-form-item>

    <el-form-item label="下单日期" prop="order_status">
      <el-date-picker v-model="purchaseForm.order_status" type="date"></el-date-picker>
    </el-form-item>

    <el-form-item label="单位名称" prop="unit_name">
      <el-input v-model="purchaseForm.unit_name" placeholder="请填写单位名称"></el-input>
    </el-form-item>
    <el-form-item label="单位地址" prop="address">
      <el-input v-model="purchaseForm.address" placeholder="请填写单位地址"></el-input>
    </el-form-item>

    <!-- 产品信息列表 -->
    <el-table :data="productTableData" style="width: 100%" border>
      <el-table-column label="产品编码">
        <template #default="scope">
          <el-input v-model="scope.row.productCode" placeholder="输入产品编码"></el-input>
        </template>
      </el-table-column>
      <el-table-column label="产品名称">
        <template #default="scope">
          <el-input v-model="scope.row.name" placeholder="输入产品名称"></el-input>
        </template>
      </el-table-column>
      <el-table-column label="单价">
        <template #default="scope">
          <el-input-number v-model="scope.row.unitPrice" :min="0" disabled></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="数量">
        <template #default="scope">
          <el-input-number v-model="scope.row.count" :min="0"></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="操作">
        <template #default="scope">
          <el-button @click="deleteProduct(scope.$index)" type="danger">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-button type="primary" @click="addNewProduct">新增产品</el-button>
  </el-form>
</template>
